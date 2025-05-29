using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class DraggableBook : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Book Settings")]
    [SerializeField] private bool isLever = false;
    [SerializeField] private float removeThreshold = 100f; // пиксели в UI для удаления
    [SerializeField] private float leverThreshold  = 50f;  // пиксели для срабатывания рычага
    [SerializeField] private float destroyDelay    = 1f;   // задержка удаления
    [SerializeField] private UnityEvent onLeverPulled;     // событие при срабатывании рычага

    private Vector2   startMousePos;
    private Vector3   startLocalPos;
    private bool      dragging = false;
    private bool      removed  = false;
    private RectTransform rectTransform;
    private Rigidbody2D   rb;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rb            = GetComponent<Rigidbody2D>();
        rb.bodyType   = RigidbodyType2D.Kinematic;
        rb.simulated  = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (removed) return;
        dragging       = true;
        startMousePos  = eventData.position;
        startLocalPos  = rectTransform.localPosition;
        rb.DOKill();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || removed) return;
        float deltaX = eventData.position.x - startMousePos.x;
        rectTransform.localPosition = new Vector3(startLocalPos.x + deltaX, startLocalPos.y, startLocalPos.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!dragging || removed) return;
        dragging = false;

        float pulledDistance = Mathf.Abs(rectTransform.localPosition.x - startLocalPos.x);
        if (isLever && pulledDistance >= leverThreshold)
        {
            onLeverPulled?.Invoke();     
        }
        else if (!isLever && pulledDistance >= removeThreshold)
        {
            removed = true;
            rectTransform.SetParent(null);
            rb.simulated   = true;
            rb.bodyType    = RigidbodyType2D.Dynamic;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddForce(new Vector2(Random.Range(-0.5f,0.5f), 0), ForceMode2D.Impulse);
            Destroy(gameObject, destroyDelay);
        }
        else
        {
            // твином возвращаем на исходную позицию
            rectTransform.DOLocalMove(startLocalPos, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
        }
    }
}