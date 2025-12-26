#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleUI.Editor
{
    public static class SimpleUIInstaller
    {
        const string ConfigFolder = "Assets/SimpleUI/Configs";
        const string ConfigPath = "Assets/SimpleUI/Configs/SimpleUIConfig_Default.asset";

        [MenuItem("Tools/Simple-UI/Install or Ensure Setup", priority = 10)]
        public static void InstallOrEnsure()
        {
            EnsureFolders();

            var config = EnsureConfigAsset();
            EnsureEventSystem();

            var uiRoot = GameObject.Find("UIRoot");
            if (!uiRoot)
            {
                // Create via generator (fallback)
                CreateUIRootMenu.Create();
                uiRoot = GameObject.Find("UIRoot");
            }

            var mgr = uiRoot.GetComponent<SimpleUIManager>();
            if (!mgr) mgr = Undo.AddComponent<SimpleUIManager>(uiRoot);

            mgr.EditorAssignConfig(config);
            mgr.EditorApplyNow();

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            Selection.activeGameObject = uiRoot;
        }

        static void EnsureFolders()
        {
            if (!AssetDatabase.IsValidFolder("Assets/SimpleUI"))
                AssetDatabase.CreateFolder("Assets", "SimpleUI");

            if (!AssetDatabase.IsValidFolder("Assets/SimpleUI/Configs"))
                AssetDatabase.CreateFolder("Assets/SimpleUI", "Configs");
        }

        static SimpleUIConfig EnsureConfigAsset()
        {
            var config = AssetDatabase.LoadAssetAtPath<SimpleUIConfig>(ConfigPath);
            if (config) return config;

            config = ScriptableObject.CreateInstance<SimpleUIConfig>();
            AssetDatabase.CreateAsset(config, ConfigPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return config;
        }

        static void EnsureEventSystem()
        {
            if (Object.FindObjectOfType<EventSystem>() != null) return;
            var es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Undo.RegisterCreatedObjectUndo(es, "Create EventSystem");
        }
    }
}
#endif
