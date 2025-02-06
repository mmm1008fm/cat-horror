using System;
using GameAssembly.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameAssembly.CandleSystem
{ 
  public class Breeze : MonoBehaviour
  {
        [Tooltip("<b>X</b> - from, <b>Y</b> - to")]
        [SerializeField] private Vector2 delayRange;
        [SerializeField, Range(0, 1)] private float monsterChance;
        [SerializeField] private Monster monster;
        [SerializeField] private LayerMask dangerousLayerMask;
        [SerializeField] private Killer killer;
        [SerializeField] private CandleLighter candle;

        private float _currTime;
        private bool _isReadyToSendMonster;

        private void Awake()
        {
            _currTime = GetDelay();
        }

        private void Update()
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (_currTime <= 0)
            {
                _currTime = GetDelay();
                TrickOrTreat();
            }
            else if (!_isReadyToSendMonster)
            {
                _currTime -= Time.deltaTime;
            }
        }

        private float GetDelay() => Random.Range(delayRange.x, delayRange.y);

        private void TrickOrTreat()
        {
            float monsterSpawnChance = Random.Range(0, 1);
            if (monsterSpawnChance < monsterChance)
                SetReadyMonsterSend(true);
            else ForceBlow();
        }

        private void SetReadyMonsterSend(bool isReady) => _isReadyToSendMonster = isReady;

        private void ForceBlow()
        {
            candle.TurnOn(false);
            candle.EnableMinigame(true);
        }

        private void TrySendMonster(Vector3 startPos, Vector3[] path, Action finishCallback = null)
        {
            if (_isReadyToSendMonster)
            {
                _isReadyToSendMonster = false;
                monster.Enable(startPos, path, finishCallback);
                killer.EnableKillState(true);
            }
        }

        private void OnMonsterHide() => killer.EnableKillState(false);

        #region trigger events

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (LayerMaskUtil.ContainsLayer(dangerousLayerMask, collision.gameObject.layer))
            {
                if (collision.TryGetComponent(out DangerousZone dangerousZone))
                    TrySendMonster(dangerousZone.GetSpawnPoint(), dangerousZone.GetPath(), OnMonsterHide);
            }
        }

        #endregion
    }
}
