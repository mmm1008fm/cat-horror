using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using GameAssembly.Utils;

namespace GameAssembly.LevelSystem
{
    public class StaircaseTeleporter : MonoBehaviour
    {
        [SerializeField] private GameObject objectToTeleport;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private GameObject cameraToTeleport;
        [SerializeField] private Transform teleportDestination;
        [SerializeField] private Collider2D tpCollider;
        // [Tooltip("Текст подсказки, который выводится при подходе к двери")]
        // [SerializeField] private string promptMessage = "Нажмите E чтобы войти";

        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 1f;
        //private bool isTeleporting = false;

        private void Start()
        {
            if(fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = 0;
                fadeImage.color = c;
            }
        }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (LayerMaskUtil.ContainsLayer(playerLayer, other.gameObject.layer))
      {
        TeleportRoutine();
      }
    }
        private void TeleportRoutine()
        {
            //isTeleporting = true;
            fadeImage.DOFade(1, fadeDuration)
                .OnComplete(() =>
                {
                    fadeImage.DOFade(0, fadeDuration).OnComplete(() =>
                    {
                        //isTeleporting = false;
                    });
                    if(objectToTeleport != null && teleportDestination != null)
                    {
                        objectToTeleport.transform.position = teleportDestination.position;
                        cameraToTeleport.transform.position = teleportDestination.position;
                    }
                });

        
        }
    }
}
