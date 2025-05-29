using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace EditorAssembly
{
#if UNITY_EDITOR
    public static class SceneSwitcherUtil
    {
        private const string MAIN_SCENE_PATH = "Assets/_Presentation/Scenes/MainScene.unity";
        private const string SANDBOX_SCENE_PATH = "Assets/_Presentation/Scenes/Sandbox.unity";
        private const string MAIN_MENU_SCENE_PATH = "Assets/_Presentation/Scenes/MainMenu.unity";
        
        [MenuItem("Scenes/Switch To Main Scene")]
        public static void SwitchToMainScene()
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            EditorSceneManager.OpenScene(MAIN_SCENE_PATH);
        }

        [MenuItem("Scenes/Switch To Sandbox Scene")]
        public static void SwitchToSandboxScene()
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            EditorSceneManager.OpenScene(SANDBOX_SCENE_PATH);
        }
        
        [MenuItem("Scenes/Switch To Main Menu Scene")]
        public static void SwitchToMainMenuScene()
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            EditorSceneManager.OpenScene(MAIN_MENU_SCENE_PATH);
        }
        
        [MenuItem("Cat/Meow-Meow")]
        public static void MeowButton() => Debug.Log("Meow");
    }
#endif
}
