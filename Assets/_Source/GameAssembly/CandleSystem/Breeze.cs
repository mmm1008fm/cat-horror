using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssembly.CandleSystem
{ 
  public class Breeze : MonoBehaviour
  {
        [Tooltip("<b>X</b> - from, <b>Y</b> - to")]
        [SerializeField] private Vector2 delayRange;
        [SerializeField] private Monster monster;

        private float _currTime;
        private bool _isReadyToSendMonster;

        private void Update()
        {
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            if (_currTime <= 0)
            {
                _currTime = UnityEngine.Random.Range(delayRange.x, delayRange.y);
                SetReadyMonsterSend(true);
            }
            else if (!_isReadyToSendMonster)
            {
                _currTime -= Time.deltaTime;
            }
        }

        private void SetReadyMonsterSend(bool isReady) => _isReadyToSendMonster = isReady;

        private void SendMonster(Vector3 startPos, Vector3[] path, Action finishCallback = null)
        {
            if (_isReadyToSendMonster)
            {
                _isReadyToSendMonster = false;
                monster.Enable(true, startPos, path, finishCallback);
            }
        }

        #region trigger events
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //TODO 
        }

        private void OnTriggerExit(Collider other)
        {
            //TODO
        }
        #endregion
    }
}
