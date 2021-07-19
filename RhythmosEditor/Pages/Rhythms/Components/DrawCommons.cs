using UnityEngine;
using RhythmosEditor.UI;

namespace RhythmosEditor.Pages.Rhythms
{
    internal static class DrawCommons
    {
        public const float RulerOffset = 8;

        public const float NoteBaseSize = 64;

        public static float TimelineNoteUnit { get; internal set; }

        public static float NoteSize { get; internal set; } = 0;

        static public void OutlineBox(Rect rect, float thickness = 1, bool topBorder = false)
        {
            Rect outRect = rect;

            if (topBorder)
            {
                outRect.height = thickness;
                GUIUtils.SetColor(Colors.BoxTopColor);
                GUI.DrawTexture(outRect, Icons.Pixel);
            }

            GUIUtils.SetColor(Colors.BoxSideColor);
            outRect.height = rect.height;
            outRect.width = thickness;
            GUI.DrawTexture(outRect, Icons.Pixel);
            outRect.x = rect.xMax - thickness;
            outRect.height -= thickness;
            GUI.DrawTexture(outRect, Icons.Pixel);

            outRect = rect;
            outRect.y = rect.yMax - thickness;
            outRect.height = thickness;
            GUIUtils.SetColor(Colors.EnabledHorizontalLine);
            GUI.DrawTexture(outRect, Icons.Pixel);
        }

    }
}
