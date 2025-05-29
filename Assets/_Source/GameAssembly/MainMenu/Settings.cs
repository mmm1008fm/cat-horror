using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameAssembly.MainMenu
{
    public class Settings : MonoBehaviour
    {
        [Header("Sound")]
        [SerializeField] private Slider masterVolume;  
        [SerializeField] private Slider bgMusicVolume;
        [SerializeField] private Slider sfxVolume;
        [Header("Microphone")]
        [SerializeField] private Toggle microphoneToggle;
        [SerializeField] private TMP_Dropdown microphoneDropdown;
        [SerializeField] private Slider microphoneSensitivity;

        private void Awake()
        {
            SetupSliders();
        
            masterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
            bgMusicVolume.onValueChanged.AddListener(OnBgMusicVolumeChanged);
            sfxVolume.onValueChanged.AddListener(OnSFXVolumeChanged);
        
            DrawMicrophoneDropdown();
            microphoneDropdown.onValueChanged.AddListener(_ => OnDropdownValueChanged());
            microphoneToggle.onValueChanged.AddListener(OnUseMicrophoneChanged);
            
            microphoneSensitivity.onValueChanged.AddListener(OnSensitivitySliderValueChanged);
        
            SetupDefaults();
        }

        private void SetupDefaults()
        {
            OnDropdownValueChanged();
            OnUseMicrophoneChanged(microphoneToggle.isOn);
            
            OnSensitivitySliderValueChanged(MicrophoneSettings.DEFAULT_MICROPHONE_SENSITIVITY);
            microphoneSensitivity.value = MicrophoneSettings.DEFAULT_MICROPHONE_SENSITIVITY;
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
    
        private void OnDropdownValueChanged()
        {
            MicrophoneSettings.Instance.SetMicrophone(
                microphoneToggle.isOn ? microphoneDropdown.options[microphoneDropdown.value].text : null);
        }
    
        private void OnUseMicrophoneChanged(bool isOn) => 
            MicrophoneSettings.Instance.SetUseMicrophone(isOn);

        private void OnSensitivitySliderValueChanged(float value) => 
            MicrophoneSettings.Instance.SetMicrophoneSensitivity(value);
    }
}
