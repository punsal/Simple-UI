using UnityEngine;
using UnityEngine.UI;

namespace SimpleUI.SafeArea
{
    /// <summary>
    /// Passive debug overlay for unsafe areas (top/bottom).
    /// Driven by SimpleUIManager to avoid update-order issues in the editor.
    /// Uses normalized anchors derived from Screen.safeArea.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UnsafeAreaOverlay : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image top;
        [SerializeField] private Image bottom;

        [SerializeField] private bool show = true;

        public void SetTop(Image img) => top = img;
        public void SetBottom(Image img) => bottom = img;

        public void SetShow(bool shouldShow)
        {
            show = shouldShow;
            ApplyImmediateVisualState();
        }

        public void Apply(Rect safeArea, int screenW, int screenH)
        {
            if (!top || !bottom || screenW <= 0 || screenH <= 0)
                return;

            if (!show)
            {
                top.gameObject.SetActive(false);
                bottom.gameObject.SetActive(false);
                return;
            }

            top.gameObject.SetActive(true);
            bottom.gameObject.SetActive(true);

            // Normalize safe area boundaries (0..1)
            float safeMinY = safeArea.yMin / screenH;
            float safeMaxY = safeArea.yMax / screenH;

            // TOP unsafe = (safeMaxY..1)
            FitAnchors(top.rectTransform,
                new Vector2(0f, safeMaxY),
                new Vector2(1f, 1f));

            // BOTTOM unsafe = (0..safeMinY)
            FitAnchors(bottom.rectTransform,
                new Vector2(0f, 0f),
                new Vector2(1f, safeMinY));
        }

        private void ApplyImmediateVisualState()
        {
            if (!top || !bottom) return;
            top.gameObject.SetActive(show);
            bottom.gameObject.SetActive(show);
        }

        private static void FitAnchors(RectTransform rt, Vector2 aMin, Vector2 aMax)
        {
            rt.anchorMin = aMin;
            rt.anchorMax = aMax;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }
}
