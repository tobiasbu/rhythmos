using UnityEngine;

namespace RhythmosEditor
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
    }

    internal abstract class BaseEditorPage
    {
        public abstract void OnLoad();
        public abstract void OnPageSelect(Config config);
        public abstract void OnDraw(Rect pageRect);
    }
}
