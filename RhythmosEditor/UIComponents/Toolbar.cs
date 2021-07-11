using UnityEditor;
using UnityEngine;
using RhythmosEditor.UI;

namespace RhythmosEditor.UIComponents
{
    internal static class Toolbar
    {

        public static void Begin(float toolbarHeight = 20)
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Height(toolbarHeight), GUILayout.ExpandWidth(true));
        }

        public static void End()
        {
            GUILayout.EndHorizontal();
        }

        public static bool Button(Texture2D content, Color contentColor, params GUILayoutOption[] options)
        {
            Color oldContentColor = GUI.contentColor;
            GUI.contentColor = contentColor;
            bool result = GUILayout.Button(content, EditorStyles.toolbarButton, options);
            GUI.contentColor = oldContentColor;
            return result;
        }

        public static bool Button(GUIContent content, Color contentColor, params GUILayoutOption[] options)
        {
            Color oldContentColor = GUI.contentColor;
            GUI.contentColor = contentColor;
            bool result = GUILayout.Button(content, EditorStyles.toolbarButton, options);
            GUI.contentColor = oldContentColor;
            return result;
        }

        public static bool Button(GUIContent content, float width = 25, float height = 20)
        {
            return Button(content, Colors.Icon, GUILayout.Width(width), GUILayout.Height(height));
        }

        public static bool Button(GUIContent content, Color contentColor, float width = 25, float height = 20)
        {
            return Button(content, contentColor, GUILayout.Width(width), GUILayout.Height(height));
        }

        public static bool Toggle(bool isEnabled, GUIContent content, float width = 25, float height = 20)
        {
            Color oldBackgroundColor = GUI.backgroundColor;
            Color contentColor;
            if (isEnabled)
            {
                contentColor = Color.white;
                GUI.backgroundColor = Colors.ToggleOn;
            }
            else
            {
                contentColor = Colors.Icon;
                GUI.backgroundColor = Colors.ToggleOff;
            }

            bool result = isEnabled;
            if (Button(content, contentColor, GUILayout.Width(width), GUILayout.Height(height)))
            {
                result = !result;
            }
            GUI.backgroundColor = oldBackgroundColor;
            return result;
        }

        public static void Label(string text, params GUILayoutOption[] options)
        {

            Color oldContentColor = GUI.contentColor;
            GUI.contentColor = EditorStyles.label.normal.textColor;

            if (options != null && options.Length != 0)
            {
                GUILayout.Label(text, Styles.VerticalAlignedText, options);
            }
            else
            {
                GUILayout.Label(text, Styles.VerticalAlignedText, GUILayout.ExpandHeight(true));
            }

            GUI.contentColor = oldContentColor;
        }

        public static void Separator(float space = 2)
        {
            Color lastGuiColor = GUI.color;
            GUIUtils.SetColor(Colors.BoxSideColor);
            Rect rect = GUILayoutUtility.GetRect(space, 20, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(false));
            GUI.DrawTexture(rect, Icons.Pixel);
            GUI.color = lastGuiColor;
        }
    }
}
