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
    private float _currInterpolation;

    public void Enable(Vector3 startPos, Vector3[] path, Action finishCallback = null)
    {
      gameObject.SetActive(true);
      gameObject.transform.position = startPos;
      StartCoroutine(MoveOnPath(path, finishCallback));
    }

    private void Disable()
    {
      gameObject.SetActive(false);
    }

    private IEnumerator MoveOnPath(Vector3[] path, Action finishCallback = null)
    {
      foreach (var item in path)
      {
        float delay = MoveToPoint(item);
        yield return new WaitForSeconds(delay);
      }

      finishCallback?.Invoke();
      Disable();
    }

    private float MoveToPoint(Vector3 endPos)
    {
      var distance = Vector3.Distance(transform.position, endPos);
      float duration = distance / moveSpeed;
      transform.DOMove(endPos, duration);
      return duration;
    }
  }
}