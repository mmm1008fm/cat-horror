using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityToolbarExtender;

namespace EditorAssembly
{
#if UNITY_EDITOR
    public static class SceneSwitcherUtil
    {
        private const string MAIN_SCENE_PATH = "Assets/_Presentation/Scenes/MainScene.unity";
        private const string SANDBOX_SCENE_PATH = "Assets/_Presentation/Scenes/Sandbox.unity";
        public const string MAIN_MENU_SCENE_PATH = "Assets/_Presentation/Scenes/MainMenu.unity";
        public const string MAIN_MENU_SCENE_NAME = "MainMenu";
        
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
    
    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if(GUILayout.Button(new GUIContent(">", "Start Scene Main Menu"), ToolbarStyles.commandButtonStyle))
            {
                MySceneHelper.PlayScene(SceneSwitcherUtil.MAIN_MENU_SCENE_PATH, SceneSwitcherUtil.MAIN_MENU_SCENE_NAME);
            }
        }
    }
}

#region Stuff
static class MySceneHelper
{
    public static void PlayScene(string path, string sceneName)
    {
        if (EditorSceneManager.GetActiveScene().name != sceneName)
        {
            if (!System.IO.File.Exists(path))
            {
                Debug.LogError("Сцена не найдена: " + path);
                return;
            }

            EditorSceneManager.OpenScene(path);
        }
        
        EditorApplication.ExecuteMenuItem("Edit/Play");
    }
}

static class ToolbarStyles
{
    public static readonly GUIStyle commandButtonStyle;

    static ToolbarStyles()
    {
        commandButtonStyle = new GUIStyle("Command")
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleCenter,
            imagePosition = ImagePosition.ImageAbove,
            fontStyle = FontStyle.Bold
        };
    }
}
#endregion

#endif

