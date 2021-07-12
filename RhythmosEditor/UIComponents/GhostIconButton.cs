using UnityEngine;
using RhythmosEditor.UI;

namespace RhythmosEditor.UIComponents
{
    enum ButtonStatus
    {
        None,
        Press,
        Focus,
        Release,
        Active,
    }


    internal class GhostIconButton
    {
        private static GUIStyle ghostButtonStyle;
        private static int guiCounter = 0;
        private static int focused = int.MinValue;
        private static int controlId;
        private static ButtonStatus lastStatus;

        public static int CurrentFocused => focused;

        public static int Indexer => guiCounter;

        public static ButtonStatus LastStatus => lastStatus;

        public static void OnUpdate()
        {
            guiCounter = -1;
        }

        public static bool Draw(GUIContent iconContent, float width = 24, float height = 20)
        {
            if (ghostButtonStyle == null)
            {
                ghostButtonStyle = new GUIStyle();
                ghostButtonStyle.alignment = TextAnchor.MiddleCenter;
                ghostButtonStyle.imagePosition = ImagePosition.ImageOnly;
                ghostButtonStyle.onActive.textColor = Colors.Icon;
                ghostButtonStyle.onHover.textColor = Colors.Icon;
            }

            guiCounter++;

            lastStatus = ButtonStatus.None;

            bool inside = false;
            Color color = Colors.Icon;

            if (focused == guiCounter)
            {
                lastStatus = ButtonStatus.Focus;
                color = Colors.IconActive;
            }


            Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height));
            inside = rect.Contains(Event.current.mousePosition);

            if (GUI.enabled && inside)
            {
                if (focused < 0)
                {
                    color = Colors.IconHighlight;
                    if (GUIEvents.MouseDown)
                    {
                        lastStatus = ButtonStatus.Press;
                        focused = guiCounter;
                        controlId = GUIEvents.SetHotControl();
                        Event.current.Use();
                    }
                }
            }


            if (guiCounter == focused && GUIUtility.hotControl == controlId && Event.current.rawType == EventType.MouseUp)
            {
                focused = int.MinValue;
                lastStatus = inside ? ButtonStatus.Active : ButtonStatus.Release;
                GUIEvents.ReleaseHotControl();
                Repainter.Request();
                Event.current.Use();
            }


            if (DrawSimple(rect, iconContent, color))
            {
                lastStatus = ButtonStatus.Active;
                focused = int.MinValue;
            }

            return lastStatus == ButtonStatus.Active;
        }

        public static bool DrawSimple(Rect rect, GUIContent iconContent, Color color)
        {
            Color oldContentColor = GUI.contentColor;
            GUI.contentColor = color;
            bool result = GUI.Button(rect, iconContent, ghostButtonStyle);
            GUI.contentColor = oldContentColor;

            return result;
        }

        public static bool DrawSimple(GUIContent iconContent, Color color, float width = 24, float height = 20)
        {
            Rect rect = GUILayoutUtility.GetRect(width, height, GUILayout.Width(width), GUILayout.Height(height));
            Color oldContentColor = GUI.contentColor;
            GUI.contentColor = color;
            bool result = GUI.Button(rect, iconContent, ghostButtonStyle);
            GUI.contentColor = oldContentColor;

            return result;
        }
    }

}
