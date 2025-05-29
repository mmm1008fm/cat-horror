using UnityEngine;

namespace GameAssembly.MainMenu
{
  public class MicrophoneSettings : MonoBehaviour
  {
    public string CurrentMicrophone { get; private set; }
    public bool UseMicrophone { get; private set; }
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
  }
}