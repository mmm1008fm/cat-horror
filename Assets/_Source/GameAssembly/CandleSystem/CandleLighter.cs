using UnityEngine;

namespace GameAssembly.CandleSystem
{
  public class CandleLighter : MonoBehaviour
  {
    [SerializeField] private DynamicCandleFlicker dynamicCandleFlicker;

    public void Enable(bool enable)
    {
      gameObject.SetActive(enable);
      
    }
    
    public void TurnOnCandle() => dynamicCandleFlicker.TurnOn(true);
    public bool IsLit() => dynamicCandleFlicker.IsFlickering;
  }
}