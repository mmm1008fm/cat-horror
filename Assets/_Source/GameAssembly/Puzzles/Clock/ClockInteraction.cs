using UnityEngine;

public class ClockInteraction : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject clockUIPanel;

    [Header("Player/Keys")]
    [SerializeField] private string playerLayer = "Cat";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInside;
    private bool uiActive;

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(interactKey))
            ToggleUI();
    }

    private void ToggleUI()
    {
        uiActive = !uiActive;
        clockUIPanel.SetActive(uiActive);

        Time.timeScale = uiActive ? 0f : 1f;
        if (uiActive)
        {
            CursorManager.Instance.EnterInteractiveMode();
        }
        else
        {
            CursorManager.Instance.ExitInteractiveMode();
        }
        //Cursor.lockState = uiActive ? CursorLockMode.None : CursorLockMode.Locked;
        //Cursor.visible   = uiActive;

        // TODO: при необходимости отключайте скрипт движения игрока
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer(playerLayer))
            playerInside = false;
    }
}
