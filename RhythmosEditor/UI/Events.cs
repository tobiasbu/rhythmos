using UnityEngine;

namespace RhythmosEditor
{
    internal static partial class GUIEvents
    {
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

        public static int GetScrollDelta(Event currentEvent, Rect rect)
        {
            if (currentEvent != null)
            {
                if (IsMouseInside(rect, currentEvent))
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
                Repainter.Request();
            
            }
            return 0;
        }

    }
}
