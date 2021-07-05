using UnityEditor;
using UnityEngine;

namespace RhythmosEditor.UI
{
    internal static class Colors
    {
        public static readonly Color Selection = new Color(62f / 255f, 125f / 255f, 231f / 255f);
        public static readonly Color DarkGray = Gray(0.25f);
        public static readonly Color LightGray = Gray(0.75f);
        public static readonly Color ToggleOn = Gray(0.5f);
        public static readonly Color ToggleOff = Gray(0.95f);
        public static readonly Color LightSelection = Change(EditorStyles.label.focused.textColor, 0.25f, 1f);

        public static Color Change(Color color, float saturation, float value)
        {
            float h = 0, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            return Color.HSVToRGB(h, saturation, value);
        }

        internal static Color Gray(float level = 0.5f, float alpha = 1.0f)
        {
            return new Color(level, level, level, alpha);
        }
    }
}
