using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class DoorTeleporter : MonoBehaviour
{
    [Header("Настройки телепортации")]
    [Tooltip("Объект, который нужно телепортировать (например, игрок)")]
    [SerializeField] private GameObject objectToTeleport;
    [SerializeField] private GameObject cameraToTeleport;
    [Tooltip("Позиция, куда будет телепортирован объект. Может меняться во время игры.")]
    [SerializeField] private Transform teleportDestination;

    [Header("Настройки взаимодействия")]
    [Tooltip("Клавиша для активации телепортации")]
    [SerializeField] private KeyCode teleportKey = KeyCode.E;
    // [Tooltip("Текст подсказки, который выводится при подходе к двери")]
    // [SerializeField] private string promptMessage = "Нажмите E чтобы войти";
    [Tooltip("UI-панель с подсказкой (например, с компонентом Text), которую можно включать/выключать")]
    [SerializeField] private GameObject promptUI;

    [Header("Настройки затемнения экрана")]
    [Tooltip("Изображение (например, UI Image), которое покрывает весь экран для эффекта затемнения")]
    [SerializeField] private Image fadeImage;
    [Tooltip("Длительность эффекта затемнения/осветления")]
    [SerializeField] private float fadeDuration = 1f;

    private bool playerInRange = false;
    private bool isTeleporting = false;

    private void Start()
    {
        if(promptUI != null)
            promptUI.SetActive(false);

        if(fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }
    }

    private void Update()
    {
        if(playerInRange && !isTeleporting)
        {
            if(Input.GetKeyDown(teleportKey))
            {
                TeleportRoutine();
            }
        }
    }

    private void TeleportRoutine()
    {
        isTeleporting = true;
        fadeImage.DOFade(1, fadeDuration)
        .OnComplete(() =>
         {
            fadeImage.DOFade(0, fadeDuration).OnComplete(() =>
            {
                isTeleporting = false;
            });
            if(objectToTeleport != null && teleportDestination != null)
            {
                objectToTeleport.transform.position = teleportDestination.position;
                cameraToTeleport.transform.position = teleportDestination.position;
            }
        });

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == objectToTeleport)
        {
            playerInRange = true;
            if(promptUI != null)
            {
                Text textComp = promptUI.GetComponentInChildren<Text>();
                // if(textComp != null)
                // {
                //     textComp.text = promptMessage;
                // }
                promptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == objectToTeleport)
        {
            playerInRange = false;
            if(promptUI != null)
            {
                promptUI.SetActive(false);
            }
        }
    }
}
