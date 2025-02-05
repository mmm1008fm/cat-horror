using UnityEngine;

namespace GameAssembly.CandleSystem
{
  public class BlowListener : MonoBehaviour
  {
    [Tooltip("<b>X</b> - from, <b>Y</b> - to")]
    [SerializeField] private Vector2 thresholdVolume = new (.01f, 1f);
    [SerializeField] private float thresholdHighFreq = .05f;
    [SerializeField] private int FrequencyThreshold = 500;
    [Space]
    [SerializeField] private float blowThreshold;
    [SerializeField] private float blowAccumulatingSpeed;
    [Space]
    [SerializeField] private bool debug;

    private AudioClip _audioClip;
    private const int SampleRate = 44100;
    private string _microphone;

    private bool _isBlowing;
    private float _currBlowForce;

    private void Start()
    {
      StartMicrophone();
    }

    private void StartMicrophone()
    {
      if (Microphone.devices.Length > 0)
      {
        _microphone = Microphone.devices[0];
        _audioClip = Microphone.Start(_microphone, true, 10, SampleRate);
      }
      else
      {
        Debug.LogError("No available Microphone devices!");
      }
    }

    private void Update()
    {
      AnalyzeAudio();
      AccumulateBlow();
      AnalyzeBlows();
#if UNITY_EDITOR
      if (debug)
        Debug.Log($"BLOW FORCE: {_currBlowForce}");
#endif
    }

    private void AnalyzeAudio()
    {
      float[] samples = new float[1024];
      int position = Microphone.GetPosition(_microphone) - 1024;
      if (position < 0) return;

      _audioClip.GetData(samples, position);

      float volume = 0;
      foreach (float sample in samples)
      {
        volume += Mathf.Abs(sample);
      }

      volume /= samples.Length;

      if (IsBlowing(samples, volume))
      {
        _isBlowing = true;
        Debug.Log("Detected! You are blowing or speaking!");
      }
      else
        _isBlowing = false;
    }

    private bool IsBlowing(float[] samples, float volume)
    {
      if (volume < thresholdVolume.x || volume > thresholdVolume.y) return false;

      float highFreqEnergy = 0;
      int highFreqCount = 0;

      for (int i = 0; i < samples.Length; i++)
      {
        if (i > FrequencyThreshold) // frequencies above 1000 Hz
        {
          highFreqEnergy += Mathf.Abs(samples[i]);
          highFreqCount++;
        }
      }

      float avgHighFreqEnergy = highFreqCount > 0 ? highFreqEnergy / highFreqCount : 0;
      return avgHighFreqEnergy > thresholdHighFreq;
    }

    private void AccumulateBlow()
    {
      _currBlowForce = _isBlowing
        ? _currBlowForce + blowAccumulatingSpeed * Time.deltaTime
        : 0;
    }

    private void AnalyzeBlows()
    {
      if (_currBlowForce > blowThreshold)
      {
#if UNITY_EDITOR
        if (debug)
          Debug.Log("BLOW ACCUMULATED! TURN OFF CANDLE!");
#endif
      }
    }

    private void OnDestroy()
    {
      Microphone.End(_microphone);
    }
  }
}