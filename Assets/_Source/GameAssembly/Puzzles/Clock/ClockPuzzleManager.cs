using UnityEngine;
using DG.Tweening;

public class ClockPuzzleManager : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField] private ClockHand minuteHand;
    [SerializeField] private ClockHand hourHand;

    [Header("Правильное время")]
    [SerializeField] private int targetHour   = 3;   // 1–12
    [SerializeField] private int targetMinute = 30;  // 0–59 %5!

    [Header("UI & Game")]
    [SerializeField] private GameObject clockUIPanel; //same as ClockInteraction
    [SerializeField] private CanvasGroup messagePanel;
    
    [Header("Win-FX")]
    [SerializeField] private float shakeDuration = 3f;
    [SerializeField] private float fadeTime = 0.6f;
    [SerializeField] private float stayTime = 1.5f;

    [Header("Clock Run")]
    [SerializeField] private float minuteRevolutionTime = 60f;   // 1min per potation
    [SerializeField] private float hourRevolutionTime   = 720f;  // 12 min = 1 hour rotation

    
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
        //TODO dance!
    }

    private void PuzzleSolvedFX()
    {
        ScreenShake.Instance.Shake(shakeDuration);

        //room opened message
        if (messagePanel != null)
        {
            messagePanel.DOKill();
            DOTween.Sequence()
                   .Append(messagePanel.DOFade(1f, fadeTime).SetUpdate(true))
                   .AppendInterval(stayTime)
                   .Append(messagePanel.DOFade(0f, fadeTime).SetUpdate(true));
        }

        //resume
        Time.timeScale = 1f;
        if (clockUIPanel != null)
            clockUIPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

        //start clock running
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

        // TODO: activate secret door
    }
}
