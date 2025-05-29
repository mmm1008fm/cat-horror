using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(RectTransform), typeof(Collider2D))]
public class DraggableBook : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Book Settings")]
    [SerializeField] private bool isLever = false;
    [SerializeField] private float removeThreshold = 100f;  // пиксели для удаления обычной книги
    [SerializeField] private float leverThreshold  = 50f;   // пиксели для срабатывания рычага
    [SerializeField] private float destroyDelay    = 0.3f;  // задержка перед удалением книги

    [Header("Effects & Replacement")]
    [SerializeField] private ParticleSystem removeParticles; // ParticleSystem с Burst
    [SerializeField] private Sprite leverPulledSprite;       // новый спрайт после срабатываниия рычага

    [Header("Events")]
    [SerializeField] private UnityEvent onLeverPulled;       // событие при срабатывании рычага
    [SerializeField] private ScreenShake screenShake;
    [SerializeField] private GameObject bookshelfUIPanel;
    [SerializeField] private AudioClip victoryClip;
    [SerializeField] private TMP_Text messagePanel;
    

    private Vector2      startMousePos;
    private Vector3      startLocalPos;
    private bool         dragging = false;
    private bool         acted    = false;
    private RectTransform rectTransform;
    private UnityEngine.UI.Image uiImage;
    private AudioSource _audioSource;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        uiImage       = GetComponent<UnityEngine.UI.Image>();

        // Отключаем автоматический запуск частиц и включаем несбалированное время
        if (removeParticles != null)
        {
            var main = removeParticles.main;
            main.playOnAwake = false;
            main.useUnscaledTime = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (acted) return;
        dragging      = true;
        startMousePos = eventData.position;
        startLocalPos = rectTransform.localPosition;
        rectTransform.DOKill();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging || acted) return;
        float deltaX = eventData.position.x - startMousePos.x;
        rectTransform.localPosition = new Vector3(startLocalPos.x + deltaX, startLocalPos.y, startLocalPos.z);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!dragging || acted) return;
        dragging = false;

        float deltaX = rectTransform.localPosition.x - startLocalPos.x;
        float pulledDistance = Mathf.Abs(deltaX);

        if (isLever && pulledDistance >= leverThreshold)
        {
            acted = true;
            float dir = Mathf.Sign(deltaX);
            Vector3 leverPos = startLocalPos + Vector3.right * dir * leverThreshold;
            rectTransform
                .DOLocalMove(leverPos, 0.2f).SetEase(Ease.OutBack).SetUpdate(true)
                .OnComplete(() => rectTransform
                    .DOLocalMove(startLocalPos, 0.2f).SetEase(Ease.InBack).SetUpdate(true)
                    .OnComplete(() => {
                        if (uiImage != null && leverPulledSprite != null)
                            uiImage.sprite = leverPulledSprite;
                        onLeverPulled?.Invoke();
                    }));
            screenShake?.ShakeCamera();

            if (victoryClip != null)
                _audioSource.PlayOneShot(victoryClip);

            if (messagePanel != null)
            {
                messagePanel.DOKill();
                Sequence seq = DOTween.Sequence();
                seq.Append(messagePanel.DOFade(1f, 0.6f).SetUpdate(true))
                    .AppendInterval(1.5f)
                    .Append(messagePanel.DOFade(0f, 0.6f).SetUpdate(true));
            }

            Time.timeScale = 1f;
            if (bookshelfUIPanel != null)
                bookshelfUIPanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!isLever && pulledDistance >= removeThreshold)
        {
            acted = true;
            if (removeParticles != null)
            {
                removeParticles.transform.position = rectTransform.position;
                removeParticles.Clear(true);
                removeParticles.Play(true);
            }
            if (uiImage != null)
                uiImage.enabled = false;
            Destroy(gameObject, destroyDelay);
        }
        else
        {
            rectTransform.DOLocalMove(startLocalPos, 0.2f).SetEase(Ease.OutBack).SetUpdate(true);
        }
    }
}
