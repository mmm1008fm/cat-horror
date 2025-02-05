using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorTeleporter : MonoBehaviour
{
    [Header("Настройки телепортации")]
    [Tooltip("Объект, который нужно телепортировать (например, игрок)")]
    [SerializeField] private GameObject objectToTeleport;
    [Tooltip("Позиция, куда будет телепортирован объект. Может меняться во время игры.")]
    [SerializeField] private Transform teleportDestination;

    [Header("Настройки взаимодействия")]
    [Tooltip("Клавиша для активации телепортации")]
    [SerializeField] private KeyCode teleportKey = KeyCode.E;
    [Tooltip("Текст подсказки, который выводится при подходе к двери")]
    [SerializeField] private string promptMessage = "Нажмите E чтобы войти";
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
                StartCoroutine(TeleportRoutine());
            }
        }
    }

    private IEnumerator TeleportRoutine()
    {
        isTeleporting = true;
        // Затемнение экрана (fade out)
        yield return StartCoroutine(Fade(0, 1, fadeDuration));

        // Телепортация объекта к целевой позиции
        if(objectToTeleport != null && teleportDestination != null)
        {
            objectToTeleport.transform.position = teleportDestination.position;
        }

        // Осветление экрана (fade in)
        yield return StartCoroutine(Fade(1, 0, fadeDuration));

        isTeleporting = false;
    }

    // Корутин для плавного изменения прозрачности fadeImage
    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        if(fadeImage == null)
            yield break;

        float elapsed = 0f;
        Color c = fadeImage.color;
        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            c.a = alpha;
            fadeImage.color = c;
            yield return null;
        }
        c.a = endAlpha;
        fadeImage.color = c;
    }

    // При входе в зону двери (триггер)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, является ли вошедший объект нужным (например, игроком)
        if(collision.gameObject == objectToTeleport)
        {
            playerInRange = true;
            if(promptUI != null)
            {
                // Если на UI-панели есть компонент Text, обновляем текст подсказки
                Text textComp = promptUI.GetComponentInChildren<Text>();
                if(textComp != null)
                {
                    textComp.text = promptMessage;
                }
                promptUI.SetActive(true);
            }
        }
    }

    // При выходе из зоны двери (триггер)
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
