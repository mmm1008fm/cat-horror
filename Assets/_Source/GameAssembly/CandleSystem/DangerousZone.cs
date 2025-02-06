using UnityEngine;

namespace GameAssembly.CandleSystem
{
  public class DangerousZone : MonoBehaviour
  {
    [SerializeField] private Transform monsterSpawnPoint;
    [SerializeField] private Transform[] monsterPath;

    public Vector3 GetSpawnPoint() => monsterSpawnPoint.position;

    public Vector3[] GetPath()
    {
      Vector3[] path = new Vector3[monsterPath.Length];
      for (int i = 0; i < monsterPath.Length; i++)
      {
        path[i] = monsterPath[i].position;
      }

      return path;
    }
  }
}