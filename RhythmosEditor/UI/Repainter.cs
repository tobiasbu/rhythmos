using UnityEditor;

namespace RhythmosEditor.UI
{
    internal static class Repainter
    {
        public static bool ShouldRepaint { get; private set; } = false;

        public static void Request()
        {
            ShouldRepaint = true;
        }

        public static void Clear()
        {
            ShouldRepaint = false;
        }

        internal static void RepaintFocused()
        {
            if (EditorWindow.focusedWindow != null)
            {
                EditorWindow.focusedWindow.Repaint();
            }
        }
    }
}
