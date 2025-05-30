using GameAssembly.MainMenu;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [Header("Cursor Textures")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D pressedCursor;
    [SerializeField] private Vector2 hotspot = new Vector2(16,16);  //click position

    [Header("Pause Key")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private bool _isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        ApplyDefaultCursor();
        HideCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
            TogglePause();
    }
    
    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void ApplyDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }
    
    public void ApplyPressedCursor()
    {
        Cursor.SetCursor(pressedCursor, hotspot, CursorMode.Auto);
    }
    
    public void EnterInteractiveMode()
    {
        ShowCursor();
        ApplyDefaultCursor();
    }
    
    public void ExitInteractiveMode()
    {
        HideCursor();
    }
    
    public void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;

        if (_isPaused)
        {
            Settings.Instance.Show();
            ShowCursor();
            ApplyDefaultCursor();
        }
        else
        {
            ExitInteractiveMode();
            Settings.Instance.Hide();
        }
    }
}
