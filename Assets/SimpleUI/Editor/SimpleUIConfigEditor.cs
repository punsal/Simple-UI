#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SimpleUI.Editor
{
    [CustomEditor(typeof(SimpleUIConfig))]
    public sealed class SimpleUIConfigEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUILayout.Space(8);

            if (GUILayout.Button("Apply To Scene (UIRoot)"))
            {
                var uiRoot = GameObject.Find("UIRoot");
                if (!uiRoot)
                {
                    Debug.LogWarning("[Simple-UI] No UIRoot found in scene.");
                    return;
                }

                var mgr = uiRoot.GetComponent<SimpleUIManager>();
                if (!mgr)
                {
                    Debug.LogWarning("[Simple-UI] UIRoot has no SimpleUIManager.");
                    return;
                }

                mgr.EditorAssignConfig((SimpleUIConfig)target);
                mgr.EditorApplyNow();
            }
        }
    }
}
#endif
