using UnityEngine;

namespace SimpleUI.Layout
{
    public static class DeviceLayout
    {
        // Portrait-only assumption.
        public static float Aspect => (float)Screen.height / Screen.width;

        // Tune this threshold to your taste.
        public static bool IsTabletLike => Aspect < 1.6f;
    }
}