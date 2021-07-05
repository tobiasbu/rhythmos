using UnityEditor;
using UnityEngine;
using RhythmosEditor.UI;
using RhythmosEditor.Utils;

namespace RhythmosEditor.Pages.Rhythms
{
    static internal class TrackDraw
    {

        private const float rulerBorder = 1;
        public static float noteBaseSize = 32;

        static public void Ruler(float width, float height = 20)
        {

            Rect rect = GUILayoutUtility.GetRect(width, height);

            GUI.color = Colors.Gray(1, 0.1f);
            GUI.DrawTexture(rect, Icons.Pixel);

            #region Outline box
            GUI.color = DrawCommons.BoxBottomColor;
            Rect outRect = rect;
            outRect.y = rect.y + rect.height - rulerBorder;
            outRect.height = rulerBorder;
            GUI.DrawTexture(outRect, Icons.Pixel);

            #endregion

            #region Track intervals
            GUI.BeginClip(rect);

            float baseSize = noteBaseSize * 5;
            int secondInterval = Mathf.CeilToInt((width - DrawCommons.RulerOffset) / baseSize);
            float labelTimer = 0;
            int i, j;
            Rect intervalRect = new Rect(DrawCommons.RulerOffset, rect.height - 10,1,10);
            Rect semiIntervalRect = intervalRect;
            semiIntervalRect.height = 3;
            semiIntervalRect.y = rect.height - semiIntervalRect.height;

            for (i = 0; i < secondInterval; i += 1)
            {
                GUI.color = Color.gray;
                for (j = 1; j < 5; j += 1)
                {
                    semiIntervalRect.x += baseSize * 0.2f;
                    GUI.DrawTexture(semiIntervalRect, Icons.Pixel);
                }

                GUI.color = Color.black;
                GUI.DrawTexture(intervalRect, Icons.Pixel);
                GUI.Label(new Rect(intervalRect.x + 1, intervalRect.y - 8, 64, 14), StringUtils.TimeSecMilli(labelTimer), EditorStyles.miniLabel);
                intervalRect.x = baseSize * (i + 1);
                semiIntervalRect.x = intervalRect.x;
                labelTimer += 0.05f;
            }

            GUI.EndClip();
            #endregion

        }
    }
}
