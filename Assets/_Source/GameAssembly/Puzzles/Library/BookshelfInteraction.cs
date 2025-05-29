using UnityEngine;


public class BookshelfInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private GameObject bookshelfUIPanel;
    [SerializeField] private string playerLayer = "Cat";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInside;
    private bool uiActive;

    private void Awake()
    {
        if (bookshelfUIPanel != null)
            bookshelfUIPanel.SetActive(false);
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(interactKey))
            ToggleUI();
    }

    private void ToggleUI()
    {
        uiActive = !uiActive;
        if (bookshelfUIPanel != null)
            bookshelfUIPanel.SetActive(uiActive);

        Time.timeScale = uiActive ? 0f : 1f;

        if (uiActive)
            CursorManager.Instance.EnterInteractiveMode();
        else
            CursorManager.Instance.ExitInteractiveMode();
    }
    public void OnPointerDown() 
    {
        if (uiActive)
        {
            CursorManager.Instance.ApplyPressedCursor();
        }
    }

    public void OnPointerUp()
    {
        if (uiActive)
        {
            CursorManager.Instance.ApplyDefaultCursor();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            playerInside = false;
    }
}
