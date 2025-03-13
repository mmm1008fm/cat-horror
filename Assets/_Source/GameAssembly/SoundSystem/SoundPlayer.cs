using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Volume in-Editor Settings")]
    [SerializeField, Range(0, 1)] private float volume;

    private readonly string _masterVolumeExposedName = "MasterVolume";

#region Singleton

    private static SoundPlayer _instance;
    public static SoundPlayer Instance => _instance;
#endregion
    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);
        else
        {
            _instance = new SoundPlayer();
            LoadVolumeSettings();
            DontDestroyOnLoad(this);
        }
    }

 #region Playing Sound
    public void PlaySound(AudioSource source, AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    public void PlaySound(AudioClip clip) => PlaySound(defaultAudioSource, clip);
    #endregion

 #region Volume

    private void LoadVolumeSettings()
    {
        if (PlayerPrefs.HasKey(_masterVolumeExposedName))
            SetVolume(PlayerPrefs.GetFloat(_masterVolumeExposedName));
    }

    /// <param name="volume">from 0 to 1 in %</param>
    private void SetVolume(float volume)
    {
        float value = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(_masterVolumeExposedName, value);
    }

    /// <param name="value">straight value from mixer</param>
    private void SaveVolume(float value)
    {
        PlayerPrefs.SetFloat(_masterVolumeExposedName, value);
        PlayerPrefs.Save();
    }

#if UNITY_EDITOR
    private void Update()
    {
        SetVolume(volume);
    }
#endif
    #endregion
}
