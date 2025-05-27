using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class ClockHand : MonoBehaviour,IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public event Action OnHandReleased;

    [Header("Setup")]
    [SerializeField] private bool isMinuteHand = true;
    [SerializeField] private Transform pivot;
    [SerializeField] private float snapStepDeg = 30f;
    [SerializeField] private float snapDuration = 0.2f;
    
    [HideInInspector] public int CurrentHour;                 // 1-12
    [HideInInspector] public int CurrentMinute;               // 0-59

    private bool dragging;

    private void Awake()
    {
        if (pivot == null) pivot = transform.parent;
    }

    public void OnPointerDown(PointerEventData _) 
    {
        dragging = true;
        transform.DOKill();
    }

    public void OnDrag(PointerEventData e)
    {
        if (!dragging) return;

        Vector2 dir = e.position - (Vector2)Camera.main.WorldToScreenPoint(pivot.position);
        float ang  = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        ang        = (ang + 360f) % 360f;

        transform.localEulerAngles = new Vector3(0, 0, ang);
    }

    public void OnPointerUp(PointerEventData _)
    {
        if (!dragging) return;
        dragging = false;

        //angle scrapping
        float currentAng = transform.localEulerAngles.z;
        float snapped   = Mathf.Round(currentAng / snapStepDeg) * snapStepDeg;

        //magnitising
        transform
          .DOLocalRotate(new Vector3(0, 0, snapped), snapDuration)
          .SetEase(Ease.OutBack)
          .SetUpdate(true)
          .OnComplete(() =>
          {
              //update
              if (isMinuteHand)
              {
                  CurrentMinute = Mathf.RoundToInt((360 - (snapped % 360f)) / 6f);
                  Debug.Log($"Minute: {CurrentMinute}");
              }
              else
              {
                  CurrentHour = Mathf.RoundToInt((360 - (snapped % 360f) / 30f)) == 0 ? 12 : Mathf.RoundToInt((snapped % 360f) / 30f);
                  Debug.Log($"Hour: {CurrentHour}");
              }

              OnHandReleased?.Invoke();
          });
    }
}
