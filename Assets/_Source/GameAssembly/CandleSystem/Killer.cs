using System;
using System.Collections;
using GameAssembly.PlayerSystem;
using UnityEngine;

namespace GameAssembly.CandleSystem
{
  public class Killer : MonoBehaviour
  {
    [Tooltip("Kill delay if candle is already lit on monster activation")]
    [SerializeField] private float killDelay = 1f;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private CandleLighter candle;
    private bool _canKill;

    private void Awake()
    {
      candle.OnCandleTurnOn += TryKill;
    }

    private void OnEnable()
    {
      StartCoroutine(TryKillRoutine());
    }

    private IEnumerator TryKillRoutine()
    {
      if (!candle.IsLit()) yield break;
      yield return new WaitForSeconds(killDelay);
      if (!candle.IsLit()) yield break;
      TryKill();
    }

    public void EnableKillState(bool enable) => _canKill = enable;

    public void TryKill()
    {
      if (_canKill)
      {
        player.EnableMovement(false);
        //TODO take player to main menu
        Debug.Log("You lose :(");
      }
    }

    private void OnDestroy()
    {
      candle.OnCandleTurnOn -= TryKill;
    }
  }
}