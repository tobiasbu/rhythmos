using UnityEditor;
using UnityEngine;
using RhythmosEditor.UI;

namespace RhythmosEditor.Pages.Rhythms
{
    internal static class DrawCommons
    {
        private static bool alreadyLoaded;

        public const float RulerOffset = 8;
        public static Color BoxTopColor { get; private set; }
        public static Color BoxSideColor { get; private set; }
        public static Color BoxBottomColor { get; private set; }

        static public void OnLoad()
        {
            if (!alreadyLoaded)
            {
                if (EditorGUIUtility.isProSkin)
                {
                    BoxTopColor = Colors.Gray(0.137f);
                    BoxSideColor = Colors.Gray(0.157f);
                    BoxBottomColor = BoxTopColor;
                }
                else
                {
                    BoxTopColor = Colors.Gray(0.6f, 1);
                    BoxSideColor = Colors.Gray(0.647f, 1);
                    BoxBottomColor = BoxTopColor; //Colors.Gray(0.467f, 1);
                }

                alreadyLoaded = true;
            }
        }

        static public void OutlineBox(Rect rect, float thickness = 1, bool topBorder = false)
        {
            Rect outRect = rect;

            if (topBorder)
            {
                outRect.height = thickness;
                GUIUtils.SetColor(BoxTopColor);
                GUI.DrawTexture(outRect, Icons.Pixel);
            }

            GUIUtils.SetColor(BoxSideColor);
            outRect.height = rect.height;
            outRect.width = thickness;
            GUI.DrawTexture(outRect, Icons.Pixel);
            outRect.x = rect.xMax - thickness;
            outRect.height -= thickness;
            GUI.DrawTexture(outRect, Icons.Pixel);

            outRect = rect;
            outRect.y = rect.yMax - thickness;
            outRect.height = thickness;
            GUIUtils.SetColor(BoxBottomColor);
            GUI.DrawTexture(outRect, Icons.Pixel);
        }

    }
}
