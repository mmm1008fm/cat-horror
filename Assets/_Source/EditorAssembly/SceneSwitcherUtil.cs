using UnityEditor;
using UnityEditor.SceneManagement;

namespace EditorAssembly
{
    public static class SceneSwitcherUtil
    {
        private const string MAIN_SCENE_PATH = "Assets/_Presentation/Scenes/MainScene.unity";
        private const string SANDBOX_SCENE_PATH = "Assets/_Presentation/Scenes/Sandbox.unity";
        
        [MenuItem("Scenes/Switch To Main Scene")]
        public static void SwitchToMainScene() => EditorSceneManager.OpenScene(MAIN_SCENE_PATH);

        [MenuItem("Scenes/Switch To Sandbox Scene")]
        public static void SwitchToSandboxScene() => EditorSceneManager.OpenScene(SANDBOX_SCENE_PATH);
    }
}
