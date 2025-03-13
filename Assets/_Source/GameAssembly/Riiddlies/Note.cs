using UnityEngine;
using TMPro;

public class NoteController : MonoBehaviour
{
    [Header("Note UI")]
    [SerializeField] private GameObject noteUI;
    [SerializeField] private TMP_Text noteText;
    [SerializeField] private Sprite backgroundSprite;

    [Header("note settings")]
    [SerializeField] [TextArea] private string noteContent; // Текст, который будет отображаться в записке

    [Header("ui Press E")]
    [SerializeField] private GameObject activationHint;   // Объект с подсказкой (например, текст или картинка "Нажмите E")

    [Header("Player settigs")]
    [SerializeField] private string playerLayerName = "Cat"; // Имя слоя, на котором находится игрок

    private bool isPlayerInRange = false;  // Флаг, что игрок находится в зоне взаимодействия
    private bool isNoteOpen = false;       // Флаг, что записка открыта

    private void Start()
    {
        if (noteUI != null)
            noteUI.SetActive(false);
        if (activationHint != null)
            activationHint.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleNote();
        }

        if (isNoteOpen && (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f))
        {
            CloseNote();
        }
    }

    private void ToggleNote()
    {
        if (isNoteOpen)
        {
            CloseNote();
        }
        else
        {
            OpenNote();
        }
    }

    private void OpenNote()
    {
        if (noteUI != null)
        {
            noteUI.SetActive(true);
            noteText.text = noteContent;
            isNoteOpen = true;

            if (activationHint != null)
                activationHint.SetActive(false);
        }
    }

    private void CloseNote()
    {
        if (noteUI != null)
        {
            noteUI.SetActive(false);
            isNoteOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            isPlayerInRange = true;

            if (!isNoteOpen && activationHint != null)
                activationHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            isPlayerInRange = false;
            if (activationHint != null)
                activationHint.SetActive(false);
            if (isNoteOpen)
                CloseNote();
        }
    }
}
