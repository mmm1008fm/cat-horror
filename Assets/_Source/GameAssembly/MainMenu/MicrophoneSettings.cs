using UnityEngine;

namespace GameAssembly.MainMenu
{
  public class MicrophoneSettings : MonoBehaviour
  {
    public string CurrentMicrophone { get; private set; }
    public bool UseMicrophone { get; private set; }
    public float MicrophoneSensitivity { get; private set; }
    public const float DEFAULT_MICROPHONE_SENSITIVITY = 0.5f;
    #region Singleton
    private static MicrophoneSettings _instance;
    public static MicrophoneSettings Instance => _instance;
    #endregion
    private void Awake()
    {
      if (_instance != null)
      {
        Destroy(gameObject);
        return;
      }

      _instance = this;
      DontDestroyOnLoad(this);
      
      SetDefaultMicrophone();
    }
    
    private void SetDefaultMicrophone() => CurrentMicrophone = Microphone.devices[0];
    public void SetMicrophone(string microphone) => CurrentMicrophone = microphone;  
    public void SetUseMicrophone(bool useMicrophone) => UseMicrophone = useMicrophone;  
    public void SetMicrophoneSensitivity(float sensitivity) => MicrophoneSensitivity = 1 - sensitivity;  
  }
}