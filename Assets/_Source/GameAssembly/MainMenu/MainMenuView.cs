using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    private void Awake()
    {
        ResetSettingsPanelView();
        settingsButton.onClick.AddListener(GoToSettings);
        exitSettingsButton.onClick.AddListener(LeaveSettings);
        //TODO add subscription for play button to go to main scene
        playButton.onClick.AddListener(GoToMainScene);
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
