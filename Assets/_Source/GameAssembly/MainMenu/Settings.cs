using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private Slider masterVolume;  
    [SerializeField] private Slider bgMusicVolume;
    [SerializeField] private Slider sfxVolume;
    [Header("Microphone")]
    [SerializeField] private TMP_Dropdown microphoneDropdown;

    private void Awake()
    {
        SetupSliders();
        
        masterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
        bgMusicVolume.onValueChanged.AddListener(OnBgMusicVolumeChanged);
        sfxVolume.onValueChanged.AddListener(OnSFXVolumeChanged);
        
        DrawMicrophoneDropdown();
    }

    #region Sliders
    private void SetupSliders()
    {
        SetupSlider(masterVolume, SoundPlayer.Instance.GetSavedMasterVolume());
        SetupSlider(bgMusicVolume, SoundPlayer.Instance.GetSavedBgMusicVolume());
        SetupSlider(sfxVolume, SoundPlayer.Instance.GetSavedSFXVolume());
    }
    
    private void SetupSlider(Slider slider, float value) => slider.value = value;

    private void OnMasterVolumeChanged(float value) => SoundPlayer.Instance.SetMasterVolume(value);
    private void OnBgMusicVolumeChanged(float value) => SoundPlayer.Instance.SetBgMusicVolume(value);
    private void OnSFXVolumeChanged(float value) => SoundPlayer.Instance.SetSFXVolume(value);
    #endregion

    private void DrawMicrophoneDropdown()
    {
        microphoneDropdown.ClearOptions();
        microphoneDropdown.AddOptions(Microphone.devices.ToList());
    }
}
