using UnityEngine;
using UnityEngine.UI;

namespace SimpleUI.SafeArea
{
    /// <summary>
    /// Visualizes the unsafe area as 4 UI Images (top/bottom/left/right).
    /// Put this on a GameObject under the Canvas.
    /// Assign:
    /// - rootCanvas: your Canvas
    /// - fullScreenRoot: a full-stretch RectTransform (0..1 anchors)
    /// Toggle 'show' while using Device Simulator.
    /// </summary>
    [ExecuteAlways]
    public sealed class SafeAreaDebugOverlay : MonoBehaviour
    {
        [SerializeField] private Canvas rootCanvas;
        [SerializeField] private RectTransform fullScreenRoot;
        [SerializeField] private bool show = true;

        [Header("Auto-created Images (raycast off)")]
        [SerializeField] private Image top, bottom, left, right;

        void OnEnable() => Ensure();
        void Update()
        {
            if (!show) { SetActive(false); return; }
            Ensure();
            SetActive(true);
            Layout();
        }

        private void Ensure()
        {
            if (!rootCanvas) rootCanvas = GetComponentInParent<Canvas>();
            if (!fullScreenRoot) return;

            if (top) return;
            top = Make("UnsafeTop");
            bottom = Make("UnsafeBottom");
            left = Make("UnsafeLeft");
            right = Make("UnsafeRight");
        }

        private Image Make(string name)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(fullScreenRoot, false);

            var img = go.GetComponent<Image>();
            img.raycastTarget = false;
            // Set color in Inspector if you want (default white).
            return img;
        }

        private void SetActive(bool v)
        {
            if (top) top.gameObject.SetActive(v);
            if (bottom) bottom.gameObject.SetActive(v);
            if (left) left.gameObject.SetActive(v);
            if (right) right.gameObject.SetActive(v);
        }

        private void Layout()
        {
            var sa = Screen.safeArea;

            float bottomPx = sa.y;
            float topPx = Screen.height - (sa.y + sa.height);
            float leftPx = sa.x;
            float rightPx = Screen.width - (sa.x + sa.width);

            float scale = rootCanvas ? rootCanvas.scaleFactor : 1f;
            bottomPx /= scale; topPx /= scale; leftPx /= scale; rightPx /= scale;

            Fit(top.rectTransform,    new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -topPx), Vector2.zero);
            Fit(bottom.rectTransform, new Vector2(0, 0), new Vector2(1, 0), Vector2.zero, new Vector2(0, bottomPx));
            Fit(left.rectTransform,   new Vector2(0, 0), new Vector2(0, 1), Vector2.zero, new Vector2(leftPx, 0));
            Fit(right.rectTransform,  new Vector2(1, 0), new Vector2(1, 1), new Vector2(-rightPx, 0), Vector2.zero);
        }

        private static void Fit(RectTransform rt, Vector2 aMin, Vector2 aMax, Vector2 offMin, Vector2 offMax)
        {
            rt.anchorMin = aMin;
            rt.anchorMax = aMax;
            rt.offsetMin = offMin;
            rt.offsetMax = offMax;
        }
    }
}