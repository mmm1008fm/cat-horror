using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource defaultAudioSource;
    [SerializeField] private AudioMixer audioMixer;

    private readonly string _masterVolumeExposedName = "MasterVolume";
    private readonly string _bgMusicVolumeExposedName = "BgMusicVolume";
    private readonly string _sfxVolumeExposedName = "SFXVolume";

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

    public void LoadVolumeSettings()
    {
        SetMasterVolume(GetSavedMasterVolume());
        SetBgMusicVolume(GetSavedBgMusicVolume());
        SetSFXVolume(GetSavedSFXVolume());
    }
    
    public float GetSavedMasterVolume() => GetSavedVolume(_masterVolumeExposedName);
    public float GetSavedBgMusicVolume() => GetSavedVolume(_bgMusicVolumeExposedName);
    public float GetSavedSFXVolume() => GetSavedVolume(_sfxVolumeExposedName);
    private float GetSavedVolume(string key) => PlayerPrefs.GetFloat(key);

    public void SetMasterVolume(float value) => SetVolume(_masterVolumeExposedName, value);
    public void SetBgMusicVolume(float value) => SetVolume(_bgMusicVolumeExposedName, value);
    public void SetSFXVolume(float value) => SetVolume(_sfxVolumeExposedName, value);

    /// <param name="key">mixer exposed param name</param>
    /// <param name="value">from 0 to 1 in %</param>
    private void SetVolume(string key, float value)
    {
        float tempValue = Mathf.Log10(value) * 20;
        if (value == 0) tempValue = -80;
        audioMixer.SetFloat(key, tempValue);
        SaveVolume(key, value);
    }

    /// <param name="key">player prefs key</param>
    /// <param name="value">from 0 to 1 in %</param>
    private void SaveVolume(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }
    #endregion
}