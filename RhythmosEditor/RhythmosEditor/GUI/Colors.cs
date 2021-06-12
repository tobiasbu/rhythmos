using UnityEditor;
using UnityEngine;

namespace RhythmosEditor
{
    internal static class Colors
    {
        public static readonly Color Selection = new Color(62f / 255f, 125f / 255f, 231f / 255f);
        public static readonly Color LightGray = new Color(0.75f, 0.75f, 0.75f, 1.0f);
        public static readonly Color LightSelection = ColorUtility.Change(EditorStyles.label.focused.textColor, 0.25f, 1f);
    }
}
