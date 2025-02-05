using UnityEngine;

namespace GameAssembly.Utils
{
  public static class LayerMaskUtil 
  {
    public static bool ContainsLayer(LayerMask layerMask, int layer) => 
      (layerMask.value & 1 << layer) > 0;
  }
}