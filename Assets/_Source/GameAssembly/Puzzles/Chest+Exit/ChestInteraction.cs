using UnityEngine;
using UnityEngine.EventSystems;

public class ChestInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private GameObject chestUIPanel;     // UI панель сундука
    [SerializeField] private GameObject interactionHint;  // подсказка «Нажмите E» на полу
    [SerializeField] private string playerLayer = "Cat";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Chest Contents")]
    [SerializeField] private GameObject crowbarButton;   // UI кнопка/иконка ломика внутри панели

    public static bool HasCrowbar { get; private set; }  // флаг наличия ломика

    private bool _playerInRange;
    private bool _uiOpen;

    private void Awake()
    {
        if (chestUIPanel != null)
            chestUIPanel.SetActive(false);
        if (interactionHint != null)
            interactionHint.SetActive(false);
        HasCrowbar = false;

        if (crowbarButton != null)
            crowbarButton.SetActive(true);
    }

    private void Update()
    {
        if (!_playerInRange) return;
        if (Input.GetKeyDown(interactKey))
            ToggleChestUI();
    }

    private void ToggleChestUI()
    {
        _uiOpen = !_uiOpen;
        if (chestUIPanel != null)
            chestUIPanel.SetActive(_uiOpen);
        if (interactionHint != null)
            interactionHint.SetActive(!_uiOpen);

        Time.timeScale = _uiOpen ? 0f : 1f;
        if (_uiOpen)
            CursorManager.Instance.EnterInteractiveMode();
        else
            CursorManager.Instance.ExitInteractiveMode();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayer))
        {
            _playerInRange = true;
            if (!_uiOpen && interactionHint != null)
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
            if (_uiOpen)
                ToggleChestUI();
        }
    }

    public void OnCrowbarClicked()
    {
        if (HasCrowbar) return;
        HasCrowbar = true;
        if (crowbarButton != null)
            crowbarButton.SetActive(false);
    }
}
