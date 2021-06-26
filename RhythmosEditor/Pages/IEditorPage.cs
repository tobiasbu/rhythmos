using UnityEngine;
using RhythmosEditor.Settings;

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
        void OnLoad();
        void OnPageSelect(RhythmosConfig config);
        void OnDraw(Rect pageRect);
    }
}
