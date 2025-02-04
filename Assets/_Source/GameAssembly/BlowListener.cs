using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlowListener : MonoBehaviour
{
    [SerializeField] private float thresholdVolume = .02f;
    [SerializeField] private float thresholdHighFreq = .005f;
    private AudioClip audioClip;
    private const int SampleRate = 44100; // Частота дискретизации
    private string microphone;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            microphone = Microphone.devices[0]; 
            audioClip = Microphone.Start(microphone, true, 10, SampleRate);
        }
        else
        {
            Debug.LogError("No available Microphone devices!");
        }
    }

    void Update()
    {
        AnalyzeAudio();
    }

    void AnalyzeAudio()
    {
        float[] samples = new float[1024];
        int position = Microphone.GetPosition(microphone) - 1024; // Получаем текущую позицию
        if (position < 0) return; // Если позиция меньше нуля, выходим

        audioClip.GetData(samples, position);

        float volume = 0;
        foreach (float sample in samples)
        {
            volume += Mathf.Abs(sample);
        }
        volume /= samples.Length;

        if (IsBlowing(samples, volume))
        {
            Debug.Log("Игрок дует в микрофон!");
        }
    }

    bool IsBlowing(float[] samples, float volume)
    {
        if (volume < thresholdVolume) return false;

        // Проверка наличия высоких частот
        float highFreqEnergy = 0;
        int highFreqCount = 0;

        for (int i = 0; i < samples.Length; i++)
        {
            if (i > 500) // Например, рассматриваем частоты выше 1000 Гц
            {
                highFreqEnergy += Mathf.Abs(samples[i]);
                highFreqCount++;
            }
        }

        // Если энергия высоких частот превышает определенный порог
        float avgHighFreqEnergy = highFreqCount > 0 ? highFreqEnergy / highFreqCount : 0;
        return avgHighFreqEnergy > thresholdHighFreq;
    }

    void OnApplicationQuit()
    {
        Microphone.End(microphone);
    }
}

