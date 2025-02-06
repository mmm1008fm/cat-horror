using System;
using GameAssembly.Utils;
using UnityEngine;

namespace GameAssembly.CandleSystem
{ 
  public class Breeze : MonoBehaviour
  {
        [Tooltip("<b>X</b> - from, <b>Y</b> - to")]
        [SerializeField] private Vector2 delayRange;
        [SerializeField] private Monster monster;
        [SerializeField] private LayerMask dangerousLayerMask;
        [SerializeField] private Killer killer;

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
                SetReadyMonsterSend(true);
            }
            else if (!_isReadyToSendMonster)
            {
                _currTime -= Time.deltaTime;
            }
        }

        private float GetDelay() => UnityEngine.Random.Range(delayRange.x, delayRange.y);

        private void SetReadyMonsterSend(bool isReady) => _isReadyToSendMonster = isReady;

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
