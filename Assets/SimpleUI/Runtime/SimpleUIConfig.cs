using UnityEngine;

namespace SimpleUI
{
    [CreateAssetMenu(menuName = "Simple-UI/Simple UI Config", fileName = "SimpleUIConfig")]
    public sealed class SimpleUIConfig : ScriptableObject
    {
        public enum Stage
        {
            Dev,
            Prod
        }

        [Header("Stage")]
        public Stage stage = Stage.Dev;

        [Header("Canvas Scaler")]
        public Vector2 referenceResolution = new Vector2(1080, 2400);
        [Range(0f, 1f)] public float matchWidthOrHeight = 0.5f;

        [Header("Bars (VerticalLayoutGroup)")]
        public float topBarPreferredHeight = 160f;
        public float bottomBarPreferredHeight = 200f;

        [Header("Safe Area Debug Overlay")]
        public bool showUnsafeOverlayInDev = true;
        public bool showUnsafeOverlayInProd = false;

        [Header("Perf")]
        [Tooltip("Dev: keep updating (respond to simulator device changes). Prod: apply once and stop polling.")]
        public bool freezeInProd = true;
    }
}
