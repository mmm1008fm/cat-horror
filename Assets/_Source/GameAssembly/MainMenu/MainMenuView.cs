using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameAssembly.MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private float fadeDuration;
    [Header("Settings")]
    [SerializeField] private CanvasGroup settingsPanel;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitSettingsButton;
    [Space, SerializeField] private Settings settings;

    private bool _isSettingsInited;
    public void Awake()
    {
        settingsButton.onClick.AddListener(GoToSettings);
        exitSettingsButton.onClick.AddListener(LeaveSettings);
        playButton.onClick.AddListener(GoToMainScene);
        ResetSettingsPanelView();
    }

    private void Update()
    {
        if (!_isSettingsInited)
        {
            _isSettingsInited = true;
            settings.Init();
        }
    }

    private void GoToMainScene()
    {
        SceneManager.LoadScene(1);
    }

    private void ResetSettingsPanelView()
    {
        settingsPanel.alpha = 0;
        settingsPanel.gameObject.SetActive(false);
    }

    private void GoToSettings()
    {
        settingsPanel.gameObject.SetActive(true);
        settingsPanel.DOFade(1, fadeDuration);
    }

    private void LeaveSettings()
    {
        settingsPanel.DOFade(0, fadeDuration)
            .OnComplete(() => 
                settingsPanel.gameObject.SetActive(false));
    }
}
