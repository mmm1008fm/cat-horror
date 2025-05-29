using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameAssembly.CombinationLockSystem
{
  public class CombinationLock : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private Button backspaceBtn;
    [SerializeField] private Button entereBtn;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private int maxLength;
    [SerializeField] private string correctPassword;
    [Header("Sound feedback")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip winSound;
    [Header("Level feedback")]
    [SerializeField] private List<GameObject> toOn;
    [SerializeField] private Transform movable;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float duration;

    
    private readonly List<int> _input = new();
    private int _correctPassword;
    private void Awake()
    {
      _correctPassword = int.Parse(correctPassword);
      for (int i = 0; i < buttons.Count; i++)
      {
        int index = i;
        buttons[i].onClick.AddListener(() => OnButtonPressed(index));
      }
      
      backspaceBtn.onClick.AddListener(ClearInput);
      entereBtn.onClick.AddListener(ValidateInput);
      DrawInput();
    }

    private void OnButtonPressed(int value)
    {
      if (_input.Count < maxLength)
      {
        _input.Add(value);
        DrawInput();
      }
    }

    private void ValidateInput()
    {
      if (_input.Count == maxLength)
      {
        bool isCorrect = true;
        int temp = _correctPassword;
        for (int i = 0; i < maxLength; i++)
        {
          if (_input[_input.Count - i - 1] == temp % 10)
          {
            temp /= 10;
          }
          else
          {
            isCorrect = false;
            SoundPlayer.Instance.PlaySound(source, loseSound);
            return;
          }
        }

        SoundPlayer.Instance.PlaySound(source, winSound);
        TurnOnObjects();
        BlockNumberButtons();
        HidePuzzleUI();
        Move();
      }
    }


    private void HidePuzzleUI() => gameObject.SetActive(false);

    private void BlockNumberButtons()
    {
      foreach (var btn in buttons)
      {
        btn.interactable = false;
      }
    }

    private void DrawInput()
    {
      string input = null;
      foreach (var number in _input)
      {
        input += number.ToString();
      }
      inputText.text = input;
    }

    private void ClearInput()
    {
      _input.Clear();
      DrawInput();
    }

    #region Level
    private void Move()
    {
      if (movable)
        movable.DOMove(targetPosition.position, duration);
    }

    private void TurnOnObjects()
    {
      foreach (var obj in toOn)
        obj.SetActive(true);
    }
    #endregion
  }
}
