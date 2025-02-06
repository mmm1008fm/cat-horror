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

    private void Update()
    {
      #if UNITY_EDITOR
      DebugTurnOff();
      #endif
    }
    public void Enable(bool enable)
    {
      gameObject.SetActive(enable);
      playerMovement.EnableMovement(!enable);
      matchMiniGame.SetActive(enable);
    }
    
    public void TurnOn(bool enable) => dynamicCandleFlicker.TurnOn(enable);
    public bool IsLit() => dynamicCandleFlicker.IsFlickering;
    private void DebugTurnOff()
    {
      if (Input.GetKeyDown(KeyCode.P))
        {
        TurnOn(false);
        Enable(true);
        }
    }
    private IEnumerator DelayedDisable(bool enable, float delay)
    {
        yield return new WaitForSeconds(delay);
        Enable(false);
    }
    public void Disable() => StartCoroutine(DelayedDisable(false, disableDelay));
  }
}