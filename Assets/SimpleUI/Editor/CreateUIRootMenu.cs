#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SimpleUI.SafeArea;

namespace SimpleUI.Editor
{
    public static class CreateUIRootMenu
    {
        [MenuItem("GameObject/Simple-UI/Create UIRoot (Safe Area)", false, 10)]
        public static void Create()
        {
            var canvasGO = new GameObject("UIRoot", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster), typeof(SimpleUIManager));
            var canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 2400);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            var fullGO = new GameObject("FullScreenRoot", typeof(RectTransform));
            fullGO.transform.SetParent(canvasGO.transform, false);
            StretchToFull(fullGO.GetComponent<RectTransform>());

            var safeGO = new GameObject("SafeAreaRoot", typeof(RectTransform), typeof(SafeAreaFitter), typeof(VerticalLayoutGroup));
            safeGO.transform.SetParent(fullGO.transform, false);
            StretchToFull(safeGO.GetComponent<RectTransform>());
            ConfigureMainLayout(safeGO.GetComponent<VerticalLayoutGroup>());

            CreateBar(safeGO.transform, "TopBar", 160f);
            CreateContent(safeGO.transform, "Content");
            CreateBar(safeGO.transform, "BottomBar", 200f);

            var overlayGO = new GameObject("UnsafeAreaOverlay", typeof(RectTransform), typeof(UnsafeAreaOverlay));
            overlayGO.transform.SetParent(fullGO.transform, false);
            StretchToFull(overlayGO.GetComponent<RectTransform>());

            var topImg = CreateOverlayBand(overlayGO.transform, "UnsafeTop", new Color(1f, 1f, 0f, 0.25f));
            var bottomImg = CreateOverlayBand(overlayGO.transform, "UnsafeBottom", new Color(1f, 0f, 0f, 0.25f));

            var overlay = overlayGO.GetComponent<UnsafeAreaOverlay>();
            overlay.SetTop(topImg);
            overlay.SetBottom(bottomImg);
            overlay.SetShow(true);

            EnsureEventSystem();

            GameObjectUtility.SetParentAndAlign(canvasGO, Selection.activeGameObject);
            Undo.RegisterCreatedObjectUndo(canvasGO, "Create Simple-UI UIRoot");
            Selection.activeObject = canvasGO;
        }

        static void EnsureEventSystem()
        {
            if (Object.FindObjectOfType<EventSystem>() != null)
                return;

            var es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Undo.RegisterCreatedObjectUndo(es, "Create EventSystem");
        }

        static void ConfigureMainLayout(VerticalLayoutGroup vlg)
        {
            vlg.childAlignment = TextAnchor.UpperCenter;
            vlg.spacing = 0f;
            vlg.padding = new RectOffset(0, 0, 0, 0);
            vlg.childControlWidth = true;
            vlg.childControlHeight = true;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight = false;
        }

        static void CreateBar(Transform parent, string name, float preferredHeight)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            var le = go.GetComponent<LayoutElement>();
            le.preferredHeight = preferredHeight;
            le.flexibleHeight = 0f;
            StretchX(go.GetComponent<RectTransform>());
        }

        static void CreateContent(Transform parent, string name)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(LayoutElement));
            go.transform.SetParent(parent, false);
            var le = go.GetComponent<LayoutElement>();
            le.flexibleHeight = 1f;
            StretchX(go.GetComponent<RectTransform>());
        }

        static Image CreateOverlayBand(Transform parent, string name, Color imgColor)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);

            var img = go.GetComponent<Image>();
            img.raycastTarget = false;
            img.color = imgColor;

            StretchToFull(go.GetComponent<RectTransform>());
            return img;
        }

        static void StretchToFull(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        static void StretchX(RectTransform rt)
        {
            rt.anchorMin = new Vector2(0, 0.5f);
            rt.anchorMax = new Vector2(1, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMin = new Vector2(0, rt.offsetMin.y);
            rt.offsetMax = new Vector2(0, rt.offsetMax.y);
        }
    }
}
#endif
