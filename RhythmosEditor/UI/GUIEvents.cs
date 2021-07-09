using UnityEngine;

namespace RhythmosEditor
{
    internal static partial class GUIEvents
    {
        private static bool holdMouseDown;
        private static int focusedControlId;


        public static bool HoldMouseDown {
            get {
                return holdMouseDown;
            }
        }

        public static bool MouseDown {
            get {
                if (Event.current != null)
                {
                    return Event.current.type == EventType.MouseDown && Event.current.button == 0;
                }
                return false;
            }
        }


        internal static void OnUpdate()
        {
            if (Event.current != null)
            {
                Event current = Event.current;

                if (!holdMouseDown)
                {
                    holdMouseDown |= (current.type == EventType.MouseDown && current.button == 0);
                }
                else
                {
                    holdMouseDown &= (current.type != EventType.MouseUp || current.button != 0);
                }

                if (Event.current.rawType == EventType.MouseUp)
                {
                    holdMouseDown = false;
                }

            }
        }

        internal static int SetHotControl(Rect rect, FocusType focusType = FocusType.Passive)
        {
            focusedControlId = GUIUtility.GetControlID(focusType, rect);
            GUIUtility.hotControl = focusedControlId;
            return focusedControlId;
        }

        internal static int SetHotControl(FocusType focusType = FocusType.Passive)
        {
            focusedControlId = GUIUtility.GetControlID(focusType);
            GUIUtility.hotControl = focusedControlId;
            return focusedControlId;
        }

        internal static void ReleaseHotControl()
        {
            focusedControlId = 0;
            GUIUtility.hotControl = 0;
        }

        public static bool IsMouseInside(Rect box, Event currentEvent)
        {
            if (currentEvent != null)
            {
                return box.Contains(currentEvent.mousePosition);
            }

            return false;
        }

        public static bool IsMouseInside(Rect box)
        {
            return IsMouseInside(box, Event.current);
        }

        public static int GetScrollDelta(Event currentEvent)
        {
            if (currentEvent != null)
            {
                if (currentEvent.type == EventType.ScrollWheel)
                {

                    if (currentEvent.delta.y > 0)
                    {
                        return 1;
                    }
                    else if (currentEvent.delta.y < 0)
                    {
                        return -1;
                    }


                }
            }
            return 0;
        }
    }
}
