
using UnityEngine;
using UnityEditor;
using RhythmosEditor.UI;

namespace RhythmosEditor.UIComponents
{
    internal static class Styles
    {
        public static GUIStyle ListLabel { private set; get; }
        public static GUIStyle MiniLabel { private set; get; }

        public static GUIStyle WhiteMiniLabel { private set; get; }

        public static GUIStyle VerticalAlignedText { private set; get; }

        public static GUIStyle Header { private set; get; }
        public static GUIStyle LinkLabel { get; private set; }

        internal static void Load()
        {
            if (ListLabel == null)
            {
                ListLabel = new GUIStyle("label");
                GUIStyleState normalStyle = EditorStyles.label.normal;
                ListLabel.normal = normalStyle;
                ListLabel.normal.textColor = Color.white;
                ListLabel.contentOffset = new Vector2(2, -1);
            }

            if (MiniLabel == null)
            {
                MiniLabel = new GUIStyle(EditorStyles.miniLabel);
                MiniLabel.alignment = TextAnchor.MiddleLeft;
                MiniLabel.padding.left = 0;
                MiniLabel.padding.right = 0;
                MiniLabel.contentOffset = Vector2.zero;

            }

            if (WhiteMiniLabel == null)
            {
                WhiteMiniLabel = new GUIStyle(EditorStyles.whiteMiniLabel);
                WhiteMiniLabel.padding.left = 0;
                WhiteMiniLabel.padding.right = 0;
                WhiteMiniLabel.contentOffset = Vector2.zero;
                WhiteMiniLabel.clipping = TextClipping.Clip;

            }

            if (VerticalAlignedText == null)
            {
                VerticalAlignedText = new GUIStyle(EditorStyles.toolbar);
                VerticalAlignedText.normal.textColor = Color.white;
                VerticalAlignedText.alignment = TextAnchor.MiddleLeft;
            }

            if (Header == null)
            {
                Header = new GUIStyle(EditorStyles.boldLabel);
                Header.fontSize = 16;
            }

            if (LinkLabel == null)
            {
                LinkLabel = new GUIStyle("label");
                LinkLabel.normal.textColor = EditorStyles.label.focused.textColor;
                LinkLabel.active.textColor = Colors.AddValue(LinkLabel.normal.textColor, -0.25f);
            }
        }
    }
}
