using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClockHand : MonoBehaviour,IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public  event Action<int> OnHandChanged;
    public  event Action      OnHandReleased;

    [Header("Setup")]
    [SerializeField] private bool isMinuteHand = true;
    [SerializeField] private Transform pivot;
    [SerializeField] private float snapStepDeg = 30f;

    public  int CurrentMinute { get; private set; }    // 0-55
    public  int CurrentHour   { get; private set; }    // 1-12

    private bool dragging;

    private void Awake()
    {
        if (pivot == null) pivot = transform.parent;
    }

    public void OnPointerDown(PointerEventData _) => dragging = true;

    public void OnDrag(PointerEventData e)
    {
        if (!dragging) return;

        Vector2 dir = e.position - (Vector2)Camera.main.WorldToScreenPoint(pivot.position);
        float ang  = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        ang        = (ang + 360f) % 360f;

        float snap = Mathf.Round(ang / snapStepDeg) * snapStepDeg;
        transform.localEulerAngles = new Vector3(0, 0, -snap);   // hour

        if (isMinuteHand)
        {
            CurrentMinute = Mathf.RoundToInt(snap / 6f);         // 6 = 1 min
            OnHandChanged?.Invoke(CurrentMinute);
        }
        else
        {
            CurrentHour = Mathf.RoundToInt(snap / 30f);          // 30 = 1 hour
            if (CurrentHour == 0) CurrentHour = 12;
        }
    }

    public void OnPointerUp(PointerEventData _)
    {
        dragging = false;
        OnHandReleased?.Invoke();
    }

    public void SetHourDirect(float hourFloat)
    {
        hourFloat      = (hourFloat + 12f) % 12f;
        CurrentHour    = Mathf.RoundToInt(hourFloat == 0 ? 12 : hourFloat);
        float angleDeg = hourFloat * 30f;
        transform.localEulerAngles = new Vector3(0, 0, -angleDeg);
    }
}
