using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameAssembly.CombinationLockSystem
{
  public class CombinationLock : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private Button backspaceBtn;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private int maxLength;
    [SerializeField] private string correctPassword;
    private List<int> _input = new();
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
      DrawInput();
    }

    private void OnButtonPressed(int value)
    {
      if (_input.Count < maxLength)
      {
        _input.Add(value);
        DrawInput();
        ValidateInput();
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
            break;
          }
        }
        //TODO paste win logic
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
  }
}
