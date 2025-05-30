using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class ExitWithCrowbar : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private string playerLayer = "Cat";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Transition Settings")]
    [SerializeField] private CanvasGroup fadePanel;       // черный экран, alpha=0
    [SerializeField] private TMP_Text needKeyText;        // "Здесь нужен ломик"
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float messageDuration = 1.5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private string sceneToLoad;

    private bool _playerInRange;

    private void Awake()
    {
        if (fadePanel != null)
            fadePanel.alpha = 0f;
        if (needKeyText != null)
            needKeyText.alpha = 0f;
    }

    private void Update()
    {
        if (!_playerInRange) return;
        if (Input.GetKeyDown(interactKey))
        {
            if (ChestInteraction.HasCrowbar)
                StartCoroutine(DoTransition());
            else
                ShowNeedKeyMessage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            _playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            _playerInRange = false;
    }

    private void ShowNeedKeyMessage()
    {
        if (needKeyText == null) return;
        needKeyText.DOKill();
        needKeyText.DOFade(1f, 0.5f).SetUpdate(true)
            .OnComplete(() => needKeyText.DOFade(0f, 0.5f).SetDelay(messageDuration).SetUpdate(true));
    }

    private IEnumerator DoTransition()
    {

        if (audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);

        if (fadePanel != null)
            fadePanel.DOFade(1f, fadeDuration).SetUpdate(true);

        yield return new WaitForSecondsRealtime(fadeDuration);

        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
    }
}
