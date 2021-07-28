using UnityEngine;
using UnityEditor;

namespace RhythmosEditor.UI
{
    internal static class Colors
    {
        private static bool alreadyLoaded;

        public static readonly Color DarkGray = Gray(0.25f);
        public static readonly Color LightGray = Gray(0.75f);
        public static readonly Color LightSelection = Change(EditorStyles.label.focused.textColor, 0.25f, 1f);

        public static Color Selection { get; private set; }
        public static Color Icon { get; private set; }
        public static Color IconHighlight { get; private set; }
        public static Color IconActive { get; private set; }
        public static Color BoxTopColor { get; private set; }
        public static Color BoxSideColor { get; private set; }
        public static Color ToggleOn { get; private set; }
        public static Color ToggleOff { get; private set; }
        public static Color DisabledHorizontalLine { get; private set; }
        public static Color EnabledHorizontalLine { get; private set; }
        public static Color Invalid { get; private set; }
        public static Color Record { get; private set; }

        public static void Load()
        {
            if (!alreadyLoaded)
            {
                if (EditorGUIUtility.isProSkin)
                {
                    Invalid = AddSat(Color.red, -0.45f);
                    Record = Invalid;
                    Icon = Gray(0.8f);
                    IconActive = Colors.Gray(0.16f);
                    BoxTopColor = Gray(0.137f);
                    BoxSideColor = Gray(0.157f);
                    DisabledHorizontalLine = Gray(0.165f);
                    ToggleOn = Gray(0.8f);
                    GUIStyle style = new GUIStyle("EyeDropperHorizontalLine");
                    Color sel = style.normal.textColor;
                    sel.a = 0.5f;
                    Selection = sel;

                }
                else
                {
                    Record = new Color(0.827f, 0.498f, 0.498f, 1f);
                    Invalid = Color.red;
                    Icon = Colors.DarkGray;
                    IconActive = AddValue(Icon, -0.16f);
                    BoxTopColor = Gray(0.6f, 1);
                    BoxSideColor = Gray(0.647f, 1);
                    DisabledHorizontalLine = Gray(0.697f, 1);
                    ToggleOn = Gray(0.5f);
                    Selection = new Color(62f / 255f, 125f / 255f, 231f / 255f);

                }
                IconHighlight = AddValue(Icon, 0.25f);
                ToggleOff = Gray(0.95f);
                EnabledHorizontalLine = BoxTopColor;

                alreadyLoaded = true;
            }
        }

        public static Color AddSat(Color color, float sat)
        {
            float h = 0, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            return Color.HSVToRGB(h, s + sat, v);
        }

        public static Color AddValue(Color color, float value)
        {
            float h = 0, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            return Color.HSVToRGB(h, s, v + value);
        }

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

        internal static Color SetAlpha(Color darkGray, float alpha)
        {
            return new Color(darkGray.r, darkGray.g, darkGray.b, alpha);
        }
    }
}
