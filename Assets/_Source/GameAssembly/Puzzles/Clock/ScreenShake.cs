using UnityEngine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    [SerializeField] private Transform camTransform;  // MainCamera
    [SerializeField] private float strength = 1.1f;
    [SerializeField] private int   vibrato  = 20;
    [SerializeField] private float randomness = 90f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Shake(float duration)
    {
        if (camTransform == null) return;
        camTransform.DOComplete(); // shake end
        camTransform.DOShakePosition(duration, strength, vibrato, randomness)
                    .OnComplete(() => camTransform.localPosition = Vector3.zero);
    }
}
