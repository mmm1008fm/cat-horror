using System;
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
    private readonly string _bgMusicVolumeExposedName = "BgMusicVolumeVolume";
    private readonly string _sfxVolumeExposedName = "SFXVolumeVolume";

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
            _instance = this;
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
        {
            SetMasterVolume(GetSavedMasterVolume());
            SetBgMusicVolume(GetSavedBgMusicVolume());
            SetSFXVolume(GetSavedSFXVolume());
        }
    }
    
    public float GetSavedMasterVolume() => GetSavedVolume(_masterVolumeExposedName);
    public float GetSavedBgMusicVolume() => GetSavedVolume(_bgMusicVolumeExposedName);
    public float GetSavedSFXVolume() => GetSavedVolume(_sfxVolumeExposedName);
    private float GetSavedVolume(string key) => PlayerPrefs.GetFloat(key);

    public void SetMasterVolume(float volume) => SetVolume(_masterVolumeExposedName, volume);
    public void SetBgMusicVolume(float volume) => SetVolume(_bgMusicVolumeExposedName, volume);
    public void SetSFXVolume(float volume) => SetVolume(_sfxVolumeExposedName, volume);

    /// <param name="key">mixer exposed param name</param>
    /// <param name="volume">from 0 to 1 in %</param>
    private void SetVolume(string key, float volume)
    {
        float value = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(key, value);
        SaveVolume(key, volume);
    }

    /// <param name="key">player prefs key</param>
    /// <param name="value">from 0 to 1 in %</param>
    private void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }

#if UNITY_EDITOR
    private void Update()
    {
        SetMasterVolume(volume);
    }
#endif
    #endregion
}
