using UnityEngine;

namespace SimpleUI.SafeArea
{
    /// <summary>
    /// Pixel insets between full screen and Screen.safeArea.
    /// Useful when you need padding math or debug info.
    /// </summary>
    public static class SafeAreaInsets
    {
        public static float BottomPx => Screen.safeArea.y;
        public static float LeftPx => Screen.safeArea.x;

        public static float TopPx
        {
            get
            {
                var sa = Screen.safeArea;
                return Screen.height - (sa.y + sa.height);
            }
        }

        public static float RightPx
        {
            get
            {
                var sa = Screen.safeArea;
                return Screen.width - (sa.x + sa.width);
            }
        }
    }
}