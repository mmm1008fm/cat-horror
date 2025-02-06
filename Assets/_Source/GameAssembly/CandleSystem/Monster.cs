using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssembly.CandleSystem
{
    public class Monster : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        public void Enable(bool enable, Vector3 startPos, Vector3[] path, Action finishCallback = null)
        {
            gameObject.SetActive(enable);
            gameObject.transform.position = startPos;
            StartCoroutine(MoveOnPath(path, finishCallback));
        }

        private IEnumerator MoveOnPath(Vector3[] path, Action finishCallback = null)
        {
            foreach (var item in path)
            {
                Tween moveTween = MoveToPoint(item);
                yield return moveTween;
            }
            finishCallback?.Invoke();
        }

        private Tween MoveToPoint(Vector3 endPos)
        {
            var distance = Vector3.Distance(transform.position, endPos);
            return transform.DOMove(endPos, distance / moveSpeed);
        }
    }
}
