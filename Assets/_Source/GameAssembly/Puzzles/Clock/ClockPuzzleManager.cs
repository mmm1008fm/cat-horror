using UnityEngine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class ClockPuzzleManager : MonoBehaviour
{
    [Header("Hands")]  
    [SerializeField] private ClockHand minuteHand;
    [SerializeField] private ClockHand hourHand;

    [Header("Target Time")]  
    [SerializeField] private int targetHour   = 3;   // 1–12
    [SerializeField] private int targetMinute = 30;  // 0–59 %5==0

    [Header("UI & Game")]  
    [SerializeField] private GameObject clockUIPanel;
    [SerializeField] private TMP_Text messagePanel;

    [Header("Screen Shake")]  
    [SerializeField] private ScreenShake screenShake;

    [Header("Message Fade")]  
    [SerializeField] private float fadeTime = 0.6f;
    [SerializeField] private float stayTime = 1.5f;

    [Header("Clock Run After Solve")]  
    [SerializeField] private float minuteRevolutionTime = 6f;
    [SerializeField] private float hourRevolutionTime   = 7f;

    [Header("Audio (Victory Sound)")]
    [SerializeField] private AudioClip victoryClip;
    private AudioSource _audioSource;

    [Header("Fireplace Objects")]
    [SerializeField] private GameObject closedFireplace;
    [SerializeField] private GameObject openFireplace;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;

        if (clockUIPanel != null)
            clockUIPanel.SetActive(false);
        if (messagePanel != null)
            messagePanel.alpha = 0f;

        if (closedFireplace != null)
            closedFireplace.SetActive(true);
        if (openFireplace != null)
            openFireplace.SetActive(false);
    }

    private void OnEnable()
    {
        minuteHand.OnHandReleased += CheckSolution;
        hourHand  .OnHandReleased += CheckSolution;
    }

    private void OnDisable()
    {
        minuteHand.OnHandReleased -= CheckSolution;
        hourHand  .OnHandReleased -= CheckSolution;
    }

    private void CheckSolution()
    {
        if (hourHand.CurrentHour == targetHour &&
            minuteHand.CurrentMinute == targetMinute)
        {
            PuzzleSolvedFX();
        }
    }

    private void PuzzleSolvedFX()
    {
        screenShake?.ShakeCamera();

        if (victoryClip != null)
            _audioSource.PlayOneShot(victoryClip);

        if (messagePanel != null)
        {
            messagePanel.DOKill();
            Sequence seq = DOTween.Sequence();
            seq.Append(messagePanel.DOFade(1f, fadeTime).SetUpdate(true))
               .AppendInterval(stayTime)
               .Append(messagePanel.DOFade(0f, fadeTime).SetUpdate(true));
        }

        Time.timeScale = 1f;
        if (clockUIPanel != null)
            clockUIPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

        if (closedFireplace != null)
            closedFireplace.SetActive(false);
        if (openFireplace != null)
            openFireplace.SetActive(true);

        minuteHand.transform
            .DOLocalRotate(new Vector3(0,0,360f), minuteRevolutionTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .SetUpdate(true);

        hourHand.transform
            .DOLocalRotate(new Vector3(0,0,360f), hourRevolutionTime, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .SetUpdate(true);
    }
}
