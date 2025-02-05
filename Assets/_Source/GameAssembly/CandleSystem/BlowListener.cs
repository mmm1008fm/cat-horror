using UnityEngine;

namespace GameAssembly.CandleSystem
{
    public class BlowListener : MonoBehaviour
    {
        [SerializeField] private float thresholdVolume = .02f;
        [SerializeField] private float thresholdHighFreq = .005f;
        private AudioClip _audioClip;
        private const int SampleRate = 44100; 
        private string _microphone;
        private const int FrequencyThreshold = 500;

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
                Debug.Log("Detected! You are blowing or speaking!");
            }
        }

        private bool IsBlowing(float[] samples, float volume)
        {
            if (volume < thresholdVolume) return false;

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

        private void OnDestroy()
        {
            Microphone.End(_microphone);
        }
    }
}

