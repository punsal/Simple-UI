using UnityEngine;

namespace SimpleUI.SafeArea
{
    /// <summary>
    /// Passive safe-area applier.
    /// This component does not run its own Update loop; it is driven by SimpleUIManager.
    /// It converts Screen.safeArea (pixels) to normalized anchors on the target RectTransform.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeAreaFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform target;

        private void Reset() => target = GetComponent<RectTransform>();

        public void Apply(Rect safeArea, int screenW, int screenH)
        {
            if (!target) target = GetComponent<RectTransform>();
            if (!target || screenW <= 0 || screenH <= 0) return;

            var min = safeArea.position;
            var max = safeArea.position + safeArea.size;

            min.x /= screenW;  min.y /= screenH;
            max.x /= screenW;  max.y /= screenH;

            target.anchorMin = min;
            target.anchorMax = max;
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;
        }
    }
}
