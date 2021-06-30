using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RhythmosEditor.UI
{
    internal static class DragDrop<T>
    {
        private static object[] dragDropItems = null;

        public static bool Valid { private set; get; } = false;

        public static bool HasValidType(object[] objectReferences, Type type)
        {
            int counter = 0;
            foreach (var value in objectReferences)
            {
                Type currentType = value.GetType();
                if (currentType.Equals(type))
                {
                    counter += 1;
                }
            }

            return counter != 0;
        }

        public static bool HasValidType(object[] objectReferences)
        {
            Type type = typeof(T);
            return HasValidType(objectReferences, type);
        }

        private static T[] Filter()
        {
            if (dragDropItems == null)
            {
                return null;
            }

            List<T> filteredItems = new List<T>();
            Type type = typeof(T);
            foreach (var value in dragDropItems)
            {
                Type currentType = value.GetType();
                if (currentType.Equals(type))
                {
                    filteredItems.Add((T)value);
                }
            }
            return filteredItems.ToArray();
        }

        public static T[] Do(Event currentEvent)
        {
            if (currentEvent != null)
            {
                switch (currentEvent.type)
                {
                    case EventType.MouseUp: // Clean up, in case MouseDrag never occurred:
                    case EventType.DragExited:
                        dragDropItems = null;
                        Valid = false;
                        DragAndDrop.PrepareStartDrag();
                        break;

                    case EventType.DragUpdated:
                        if (HasValidType(DragAndDrop.objectReferences))
                        {
                            Valid = true;
                            dragDropItems = DragAndDrop.objectReferences;
                            DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                        }
                        else
                        {
                            Valid = false;
                            dragDropItems = null;
                            DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        }
                        break;

                    case EventType.DragPerform:
                        return Filter();
                }
            }
            return null;
        }
    }


}
