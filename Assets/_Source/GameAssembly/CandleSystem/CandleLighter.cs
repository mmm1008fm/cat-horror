using System;
using GameAssembly.PlayerSystem;
using UnityEngine;
using System.Collections;

namespace GameAssembly.CandleSystem
{
  public class CandleLighter : MonoBehaviour
  {
    [SerializeField] private DynamicCandleFlicker dynamicCandleFlicker;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private GameObject matchMiniGame;
    [SerializeField] private float disableDelay = 1f;

    public event Action OnCandleTurnOn;

    public void EnableMinigame(bool enable)
    {
        playerMovement.EnableMovement(!enable);
        matchMiniGame.SetActive(enable);
    }

    private void Update()
    {
//#if UNITY_EDITOR
      DebugTurnOff();
//#endif
    }

    public void TurnOn(bool enable)
    {
      dynamicCandleFlicker.TurnOn(enable);
      if (enable) OnCandleTurnOn?.Invoke();
    }

    public bool IsLit() => dynamicCandleFlicker.IsFlickering;
    public void Disable() => StartCoroutine(DelayedEnable(false, disableDelay));

    private void DebugTurnOff()
    {
      if (Input.GetKeyDown(KeyCode.P))
      {
        TurnOn(false);
        EnableMinigame(true);
      }
    }

    private IEnumerator DelayedEnable(bool enable, float delay)
    {
        yield return new WaitForSeconds(delay);
        EnableMinigame(false);
    }
  }
}