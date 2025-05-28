using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class ScreenShake : MonoBehaviour
{
  [SerializeField] private CinemachineVirtualCamera Ccamera;
  [SerializeField] private float shakeDuration = 2f;
  [SerializeField] private float _noiseAmplitude = 3f;
  private CinemachineBasicMultiChannelPerlin _cameraNoise;
  private void Awake()
  {
    _cameraNoise = Ccamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    _cameraNoise.m_AmplitudeGain = 0f;
    //ShakeCamera();
    //StartCoroutine(ShakeCamera());
  }

  // [ContextMeuItem()]
  public void ShakeCamera()
  {
    float amplitude = 0;
    //increase amplitude
    DOTween.To(() => amplitude, x => amplitude = x, _noiseAmplitude, shakeDuration/2f)
      .OnUpdate(() =>
      {
        _cameraNoise.m_AmplitudeGain = amplitude;
      })
      //deacrease amplitude
      .OnComplete(() =>
      {
        DOTween.To(() => amplitude, x => amplitude = x, 0f, shakeDuration/2f)
          .OnUpdate(() =>
          {
            _cameraNoise.m_AmplitudeGain = amplitude;
          });
      });
  }
}