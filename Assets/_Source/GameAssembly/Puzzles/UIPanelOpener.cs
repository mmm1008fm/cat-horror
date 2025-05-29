using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class UIPanelOpener : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private GameObject panelToToggle;   // UI-панель, которую нужно показывать/скрывать
    [SerializeField] private GameObject interactionHint; // подсказка «Нажмите E» на полу
    [SerializeField] private string playerLayer = "Cat"; // слой игрока
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool _playerInRange;
    private bool _panelOpen;

    private void Awake()
    {
        if (panelToToggle != null) panelToToggle.SetActive(false);
        if (interactionHint != null) interactionHint.SetActive(false);
    }

    private void Update()
    {
        if (!_playerInRange) return;

        if (Input.GetKeyDown(interactKey))
            TogglePanel();
    }

    private void TogglePanel()
    {
        _panelOpen = !_panelOpen;

        // Show/hide the panel
        if (panelToToggle != null)
            panelToToggle.SetActive(_panelOpen);

        // Show/hide the floor hint
        if (interactionHint != null)
            interactionHint.SetActive(!_panelOpen);

        // Pause/unpause game
        Time.timeScale = _panelOpen ? 0f : 1f;

        // Cursor handling via CursorManager (if you have it)
        if (_panelOpen)
            CursorManager.Instance.EnterInteractiveMode();
        else
            CursorManager.Instance.ExitInteractiveMode();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer))
        {
            _playerInRange = true;
            if (!_panelOpen && interactionHint != null)
                interactionHint.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer))
        {
            _playerInRange = false;
            if (interactionHint != null)
                interactionHint.SetActive(false);

            // If panel left open, close it
            if (_panelOpen)
                TogglePanel();
        }
    }
}
