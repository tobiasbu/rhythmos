using System;
using UnityEngine;

namespace RhythmosEditor.UI
{
    public static class GUIUtils
    {
        private static Color oldColor;

        public static void SetColor(Color color)
        {
            oldColor = GUI.color;

            if (GUI.enabled)
            {
                GUI.color = color;
            } 
            else
            {
                color.a *= 0.675f;
                GUI.color = color;
            }

        }



        public static Color GetColor(Color color, float replaceAlpha)
        {
            color.a = replaceAlpha;
            if (!GUI.enabled) { 
                color.a *= 0.675f; 
            }
            return color;
        }

        public static Color GetColor(Color color)
        {
            if (!GUI.enabled)
            {
                color.a *= 0.675f;
            }
            return color;
        }

        public static void ResetColor()
        {
            GUI.color = oldColor;
        }

        internal static void SetColor(Color enabledColor, Color disabledColor)
        {
            oldColor = GUI.color;

            if (GUI.enabled)
            {
                GUI.color = enabledColor;
            }
            else
            {
                GUI.color = disabledColor;
            }
        }
    }
}
