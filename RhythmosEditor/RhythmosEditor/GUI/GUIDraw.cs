using UnityEditor;
using UnityEngine;

namespace RhythmosEditor
{
    internal class GUIDraw
    {
        private static Rect warningInnerRect;
        private static GUIContent undoContent = new GUIContent(Textures.Undo, "Undo");
        private static GUIContent redoContent = new GUIContent(Textures.Redo, "Redo");

        public static bool IconButton(GUIContent iconContent, params GUILayoutOption[] options)
        {
            Color oldContentColor = GUI.contentColor;

            if (EditorGUIUtility.isProSkin)
            {
                GUI.contentColor = Color.white;
            }
            else
            {
                Color col = new Color(0.25f, 0.25f, 0.25f, 1);
                GUI.contentColor = col;
            }

            bool result = GUILayout.Button(iconContent, options);

            GUI.contentColor = oldContentColor;
            return result;
        }

        public static bool IconButton(GUIContent iconContent)
        {
            return IconButton(iconContent, GUILayout.Width(28), GUILayout.Height(24));
        }

        public static void UndoRedo(IUndoRedoDelegate undoRedoDelegate, bool disableUndo, bool disableRedo)
        {
            
            GUILayout.BeginHorizontal(GUILayout.Width(56));

            if (undoRedoDelegate == null || undoRedoDelegate.UndoCount == 0)
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

            if (GUILayout.Button(undoContent, GUILayout.Width(24), GUILayout.Height(19)))
            {
                undoRedoDelegate.OnUndo();
            }

            if (undoRedoDelegate == null || undoRedoDelegate.RedoCount == 0)
                GUI.enabled = false;
            else
                GUI.enabled = true;

            if (disableRedo)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button(redoContent, GUILayout.Width(24), GUILayout.Height(19)))
            {
                undoRedoDelegate.OnRedo();
            }

            GUILayout.EndHorizontal();

            GUI.contentColor = oldColor;

            GUI.enabled = true;
        }

        static public void Box(Rect rect, Texture2D texture, float size = 1)
        {
            Box(rect, texture, Color.black);
        }

        static public void Box(Rect rect, Texture2D texture, Color color, float size = 1)
        {
            Color lastColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, size), texture);
            GUI.DrawTexture(new Rect(rect.x, rect.y + rect.height - (size / 2), rect.width, size), texture);
            GUI.DrawTexture(new Rect(rect.x, rect.y, size, rect.height), texture);
            GUI.DrawTexture(new Rect(rect.x + rect.width - (size / 2), rect.y, size, rect.height + 1), texture);
            GUI.color = lastColor;
        }

        static public void WarningBox(Rect pageRect)
        {
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
            } else
            {
                innerRect = warningInnerRect;
            }
            Rect labelRect = warningRect;
            labelRect.x = 8;
            labelRect.y = 4 + innerRect.height;
            labelRect.width = warningRect.width - 16;
            labelRect.height = warningRect.height - labelRect.y - 8;
            // GUI.Box(labelRect, "");
            GUI.skin.label.wordWrap = true;
            GUILayout.BeginArea(labelRect);
            GUILayout.Label("Rhythmos Editor is disabled due to not having RhythmosDatabase loaded.\nPlease access Settings tab and create or load a RhythmosDatabase to enable this option.", GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
            GUI.skin.label.wordWrap = false;

            GUILayout.EndArea();
        }

    }
}
