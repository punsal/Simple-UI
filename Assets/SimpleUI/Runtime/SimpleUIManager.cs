using UnityEngine;
using UnityEngine.UI;
using SimpleUI.SafeArea;

namespace SimpleUI
{
    [DisallowMultipleComponent]
    [ExecuteAlways]
    public sealed class SimpleUIManager : MonoBehaviour
    {
        [SerializeField] private SimpleUIConfig config;

        [Header("Optional refs (auto-found if null)")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private SafeAreaFitter safeAreaFitter;
        [SerializeField] private UnsafeAreaOverlay unsafeAreaOverlay;

        [Header("Layout refs (auto-found by name if null)")]
        [SerializeField] private LayoutElement topBar;
        [SerializeField] private LayoutElement bottomBar;

        private Rect _lastSafeArea;
        private Vector2Int _lastScreen;

        private void Awake()
        {
            if (!config)
            {
                Debug.LogError("[Simple-UI] SimpleUIManager has no config assigned.", this);
                return;
            }

            AutoWire();
            ApplyConfig();

            // Apply once immediately (important for first frame)
            RefreshSafeArea(force: true);

            // Prod: keep fitter ON (cheap early-out), overlay disabled by ApplyConfig/FreezeForProd
            if (config.stage == SimpleUIConfig.Stage.Prod && config.freezeInProd)
                FreezeForProd();
        }

        private void OnEnable()
        {
            if (!config) return;
            AutoWire();
            ApplyConfig();
            RefreshSafeArea(force: true);
        }

        private void OnRectTransformDimensionsChange()
        {
            // Helps in editor + device simulator resizing.
            if (!config) return;
            RefreshSafeArea(force: true);
        }

        private void Update()
        {
            if (!config) return;

            // Dev: device simulator changes while playing.
            // Prod: still fine (early-out prevents work unless values change).
            RefreshSafeArea(force: false);
        }

        private void AutoWire()
        {
            if (!canvas) canvas = GetComponentInChildren<Canvas>(true);
            if (!canvasScaler && canvas) canvasScaler = canvas.GetComponent<CanvasScaler>();

            if (!safeAreaFitter) safeAreaFitter = GetComponentInChildren<SafeAreaFitter>(true);
            if (!unsafeAreaOverlay) unsafeAreaOverlay = GetComponentInChildren<UnsafeAreaOverlay>(true);

            if (!topBar) topBar = FindLayoutElementByName("TopBar");
            if (!bottomBar) bottomBar = FindLayoutElementByName("BottomBar");
        }

        private LayoutElement FindLayoutElementByName(string name)
        {
            var trs = GetComponentsInChildren<Transform>(true);
            foreach (var t in trs)
            {
                if (t.name != name) continue;
                return t.GetComponent<LayoutElement>();
            }
            return null;
        }

        private void ApplyConfig()
        {
            if (canvasScaler)
            {
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                canvasScaler.referenceResolution = config.referenceResolution;
                canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                canvasScaler.matchWidthOrHeight = config.matchWidthOrHeight;
            }

            if (topBar) topBar.preferredHeight = config.topBarPreferredHeight;
            if (bottomBar) bottomBar.preferredHeight = config.bottomBarPreferredHeight;

            if (unsafeAreaOverlay)
            {
                unsafeAreaOverlay.SetShow(
                    config.stage == SimpleUIConfig.Stage.Dev
                        ? config.showUnsafeOverlayInDev
                        : config.showUnsafeOverlayInProd);
            }
        }

        private void FreezeForProd()
        {
            // Prod best-practice:
            // - Debug overlay OFF
            // - Keep fitter ON (cheap early-out) for rare runtime changes.
            if (unsafeAreaOverlay) unsafeAreaOverlay.enabled = false;
        }

        private void RefreshSafeArea(bool force)
        {
            AutoWire();

            var screen = new Vector2Int(Screen.width, Screen.height);
            if (screen.x <= 0 || screen.y <= 0) return;

            var sa = Screen.safeArea;

            if (!force && sa == _lastSafeArea && screen == _lastScreen)
                return;

            _lastSafeArea = sa;
            _lastScreen = screen;

            // 1) Apply safe area anchors FIRST
            if (safeAreaFitter && safeAreaFitter.enabled)
                safeAreaFitter.Apply(sa, screen.x, screen.y);

#if UNITY_EDITOR
            // 2) Rebuild layout so VerticalLayoutGroup snaps immediately in simulator/editor
            if (safeAreaFitter)
            {
                var rt = safeAreaFitter.GetComponent<RectTransform>();
                if (rt) LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
            }
            Canvas.ForceUpdateCanvases();
#endif

            // 3) Apply overlay LAST (so it matches final safe area)
            if (unsafeAreaOverlay && unsafeAreaOverlay.enabled)
                unsafeAreaOverlay.Apply(sa, screen.x, screen.y);

#if UNITY_EDITOR
            // Optional extra nudge for editor stability
            Canvas.ForceUpdateCanvases();
#endif
        }

#if UNITY_EDITOR
        public void EditorAssignConfig(SimpleUIConfig cfg)
        {
            config = cfg;
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void EditorApplyNow()
        {
            AutoWire();
            ApplyConfig();
            RefreshSafeArea(force: true);
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
