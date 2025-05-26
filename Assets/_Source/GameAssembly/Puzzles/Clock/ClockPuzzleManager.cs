using UnityEngine;
using DG.Tweening;

public class ClockPuzzleManager : MonoBehaviour
{
    [Header("Hands")]
    [SerializeField] private ClockHand minuteHand;
    [SerializeField] private ClockHand hourHand;

    [Header("Time")]
    [SerializeField] private int targetHour   = 3;   // 1-12
    [SerializeField] private int targetMinute = 30;  // multiple 5

    [Header("Win efffects")]
    [SerializeField] private float shakeDuration = 3f;
    [SerializeField] private CanvasGroup messagePanel; // message Room Open
    [SerializeField] private float fadeTime   = .6f;
    [SerializeField] private float stayTime   = 1.5f;

    private void OnEnable()
    {
        minuteHand.OnHandChanged  += SyncHourByMinute;
        minuteHand.OnHandReleased += CheckSolution;
        hourHand  .OnHandReleased += CheckSolution;
    }
    private void OnDisable()
    {
        minuteHand.OnHandChanged  -= SyncHourByMinute;
        minuteHand.OnHandReleased -= CheckSolution;
        hourHand  .OnHandReleased -= CheckSolution;
    }

    private void SyncHourByMinute(int snappedMinute)
    {
        float hFloat = hourHand.CurrentHour % 12 + snappedMinute / 60f;
        hourHand.SetHourDirect(hFloat);
    }

    private void CheckSolution()
    {
        if (hourHand.CurrentHour == targetHour &&
            minuteHand.CurrentMinute == targetMinute)
            PuzzleSolvedFX();
    }

    private void PuzzleSolvedFX()
    {
        ScreenShake.Instance.Shake(shakeDuration);

        if (messagePanel != null)
        {
            messagePanel.DOKill();
            DOTween.Sequence()
                   .Append(messagePanel.DOFade(1f, fadeTime))
                   .AppendInterval(stayTime)
                   .Append(messagePanel.DOFade(0f, fadeTime));
        }

        // TODO teleporter to secret room activate
    }
}
