using UnityEngine;
using UnityEditor;
using RhythmosEditor.UI;
using RhythmosEditor.UIComponents;

namespace RhythmosEditor
{
    internal class Components
    {

        private static Rect warningInnerRect;
        private static readonly GUIContent undoContent = new GUIContent(Icons.Undo, "Undo");
        private static readonly GUIContent redoContent = new GUIContent(Icons.Redo, "Redo");

        public static bool IconButton(GUIContent iconContent, Color contentColor, params GUILayoutOption[] options)
        {
            Color oldContentColor = GUI.contentColor;
            GUI.contentColor = contentColor;
            bool result = GUILayout.Button(iconContent, options);
            GUI.contentColor = oldContentColor;
            return result;
        }

        public static bool IconButton(Texture2D iconContent, Color contentColor, params GUILayoutOption[] options)
        {
            Color oldContentColor = GUI.contentColor;
            GUI.contentColor = contentColor;
            bool result = GUILayout.Button(iconContent, options);
            GUI.contentColor = oldContentColor;
            return result;
        }

        internal static bool IconButton(GUIContent content, float width = 28, float height = 24)
        {
            return IconButton(content, Colors.Icon, GUILayout.Width(width), GUILayout.Height(height));
        }

        internal static bool IconButton(Texture2D content, float width = 28, float height = 24)
        {
            return IconButton(content, Colors.Icon, GUILayout.Width(width), GUILayout.Height(height));
        }

        static public void OutlineBox(Rect rect, float size = 1)
        {
            OutlineBox(rect, GUI.color, size);
        }

        static public void OutlineBox(Rect rect, Color color, float size = 1)
        {
            Color lastColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, size), Icons.Pixel);
            GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - (size / 2), rect.width, size), Icons.Pixel);
            GUI.DrawTexture(new Rect(rect.x, rect.y, size, rect.height), Icons.Pixel);
            GUI.DrawTexture(new Rect(rect.x + rect.width - (size / 2), rect.y, size, rect.height + 1), Icons.Pixel);
            GUI.color = lastColor;
        }

        internal static void HorizontalLine(Vector2 position, float width, Color color, float thickness = 1)
        {
            Rect r = new Rect(position, new Vector2(width, thickness));
            Color lastColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(r, Icons.Pixel);
            GUI.color = lastColor;
        }

        internal static void HorizontalLine(Vector2 position, float width, float thickness = 1)
        {
            HorizontalLine(position, width, GUI.enabled ? Colors.EnabledHorizontalLine : Colors.DisabledHorizontalLine);
        }

        internal static void HorizontalLine(Color color, float thickness = 1)
        {
            Rect r = GUILayoutUtility.GetRect(100, thickness, GUILayout.ExpandWidth(true));
            Color lastColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(r, Icons.Pixel);
            GUI.color = lastColor;
        }

        static public void HorizontalLine(float thickness = 1)
        {
            HorizontalLine(GUI.enabled ? Colors.EnabledHorizontalLine : Colors.DisabledHorizontalLine, thickness);
        }

        internal static void VerticalLine(Vector2 position, float height, float thickness = 1)
        {
            Rect r = new Rect(position, new Vector2(thickness, height));
            Color lastColor = GUI.color;
            GUI.color = GUI.enabled ? Colors.EnabledHorizontalLine : Colors.DisabledHorizontalLine;
            GUI.DrawTexture(r, Icons.Pixel);
            GUI.color = lastColor;
        }

        public static void MiniLabel(string text, TextAnchor anchor = TextAnchor.MiddleLeft, params GUILayoutOption[] options)
        {
            Styles.MiniLabel.alignment = anchor;
            GUILayout.Label(text, Styles.MiniLabel, options);
        }

        public static void Header(string text)
        {
            GUILayout.Space(4);
            GUILayout.Label(text, Styles.Header);
            GUILayout.Space(2);

        }

        public static void LinkLabel(string text, string link)
        {

            bool click = GUILayout.Button(text, Styles.LinkLabel);

            if (click)
            {
                try
                {
                    Application.OpenURL(link);
                }
                catch
                {
                    //In most cases, the catch clause would not happen but in the interest of being thorough I will log an
                    //error and have Unity "beep" if an exception gets thrown for any reason.
                    Debug.LogError("Could not open URL. Please check your network connection and ensure the web address is correct.");
                    EditorApplication.Beep();
                }
            }
        }


        public static void DropShadowMiniLabel(Rect rect, string text, Color color)
        {
            Color lastColor = GUI.color;
            GUI.color = Color.black;
            Rect shadowRect = rect;
            shadowRect.x += 1;
            GUI.Label(shadowRect, text, Styles.WhiteMiniLabel);
            shadowRect.y += 1;
            GUI.Label(shadowRect, text, Styles.WhiteMiniLabel);
            GUI.color = color;
            GUI.Label(rect, text, Styles.WhiteMiniLabel);
            GUI.color = lastColor;
        }

        public static void UndoRedoButtons(bool disableUndo, bool disableRedo)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(56));

            if (Commands.UndoRedo.UndoCount == 0)
                GUI.enabled = false;
            else
                GUI.enabled = true;

            if (disableUndo)
            {
                GUI.enabled = false;
            }

            Color oldColor = GUI.contentColor;
            if (!EditorGUIUtility.isProSkin)
            {
                GUI.contentColor = EditorStyles.label.normal.textColor;
            }

            if (IconButton(undoContent, 26, 20))
            {
                Commands.UndoRedo.PerformUndo();
            }

            if (Commands.UndoRedo.RedoCount == 0)
                GUI.enabled = false;
            else
                GUI.enabled = true;

            if (disableRedo)
            {
                GUI.enabled = false;
            }

            if (IconButton(redoContent, 26, 20))
            {
                Commands.UndoRedo.PerformRedo();
            }

            GUILayout.EndHorizontal();

            GUI.contentColor = oldColor;

            GUI.enabled = true;
        }



        static public void WarningBox(Rect pageRect)
        {
            GUI.color = Color.white;
            Rect warningRect = pageRect;
            warningRect.width = (pageRect.width * 0.78f);
            warningRect.x = (pageRect.width * 0.5f - warningRect.width * 0.5f);
            warningRect.height = 116;
            warningRect.y = (pageRect.height * 0.45f - warningRect.height * 0.5f);

            GUI.Box(warningRect, "", GUI.skin.window);
            GUILayout.BeginArea(warningRect);

            EditorGUILayout.HelpBox("WARNING", MessageType.Warning);

            Rect innerRect;
            if (Event.current.type == EventType.Repaint)
            {
                innerRect = warningInnerRect = GUILayoutUtility.GetLastRect();
            }
            else
            {
                innerRect = warningInnerRect;
            }
            Rect labelRect = warningRect;
            labelRect.x = 8;
            labelRect.y = 4 + innerRect.height;
            labelRect.width = warningRect.width - 16;
            labelRect.height = warningRect.height - labelRect.y - 8;

            GUI.skin.label.wordWrap = true;
            GUILayout.BeginArea(labelRect);
            GUILayout.Label("Rhythmos Editor is disabled due to not having RhythmosDatabase loaded.\nPlease access Settings tab and create or load a RhythmosDatabase to enable this option.", GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
            GUI.skin.label.wordWrap = false;

            GUILayout.EndArea();
        }

    }
}
