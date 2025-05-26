using UnityEngine;

public class ClockInteraction : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject clockUIPanel;

    [Header("Player")]
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

        Cursor.lockState = uiActive ? CursorLockMode.None  : CursorLockMode.Locked;
        Cursor.visible   = uiActive;
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
