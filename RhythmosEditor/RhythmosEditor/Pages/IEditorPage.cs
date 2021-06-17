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

    internal interface IEditorPage
    {
        IUndoRedoDelegate UndoRedoDelegate { get; }
        void OnLoad();
        void OnPageSelect(Config config);
        void OnDraw(Rect pageRect);
    }
}
