using System;
using GameAssembly.Utils;
using UnityEngine;

namespace GameAssembly.CandleSystem
{
  public class Match : MonoBehaviour
  {
    [SerializeField] private LayerMask matchBoxLayer;
    [SerializeField] private float minFrictionForFire;
    [SerializeField] private float frictionAccumulationSpeed;
    [SerializeField] private CandleLighter candleLighter;
    [Space]
    [SerializeField] private ParticleSystem sparkles;
    [SerializeField] private float sparklesSpeedThreshold;
    [Space]
    [SerializeField] private bool debug;
    
    private Camera _mainCamera;
    
    private bool _isOverBox;
    private float _currFriction;

    private Vector3 _lastPosition;
    private float _dragSpeed;

    private void Awake()
    {
      _mainCamera = Camera.main;
    }

    private void OnMouseDrag()
    {
      DragMatch();
    }

    private void DragMatch()
    {
      Vector3 newPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
      newPosition.z = 0;
      transform.position = newPosition;
    }

    private void Update()
    {
      CountDragSpeed();
      AccumulateFriction();
      EmitSparkles();
      TryLightCandle();
#if UNITY_EDITOR
      if (debug)
      {
        Debug.Log($"FRICTION: {_currFriction}");
        Debug.Log($"SPEED: {_dragSpeed}");
      }
#endif
    }

    private void CountDragSpeed()
    {
      _dragSpeed = (transform.position - _lastPosition).magnitude / Time.deltaTime;
      _lastPosition = transform.position;
    }
    
    private void AccumulateFriction() =>
      _currFriction = _isOverBox && _dragSpeed > 0
        ? _currFriction + frictionAccumulationSpeed * _dragSpeed * Time.deltaTime
        : 0;

    private void TryLightCandle()
    {
      if (!candleLighter.IsLit() 
          && _currFriction >= minFrictionForFire)
        {
        candleLighter.TurnOn(true);
        candleLighter.Disable();
        }
    }

    private void EmitSparkles()
    {
      if (_isOverBox && _dragSpeed > sparklesSpeedThreshold)
        sparkles.Play();
    }

    #region trigger events

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (LayerMaskUtil.ContainsLayer(matchBoxLayer, other.gameObject.layer))
        _isOverBox = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      if (LayerMaskUtil.ContainsLayer(matchBoxLayer, other.gameObject.layer))
        _isOverBox = false;
    }

    #endregion
  }
}