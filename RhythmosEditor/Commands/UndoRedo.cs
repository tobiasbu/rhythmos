using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RhythmosEditor.Commands
{
    internal static class UndoRedo
    {
        private static RhythmosEditor editorWindow;
        private static readonly LinkedList<ICommand> UndoStack;
        private static readonly LinkedList<ICommand> RedoStack;

        public static uint MaxActions { set; get; } = 50;

        public static int UndoCount
        {
            get { return UndoStack != null ? UndoStack.Count : 0; }
        }

        public static int RedoCount
        {
            get { return RedoStack != null ? RedoStack.Count : 0; }
        }

        static UndoRedo()
        {
            UndoStack = new LinkedList<ICommand>();
            RedoStack = new LinkedList<ICommand>();
        }

        public static void SetWindow(RhythmosEditor window)
        {
           editorWindow = window;
        }
        public static void Clear()
        {
            if (UndoStack != null)
            {
                UndoStack.Clear();
            }

            if (RedoStack != null)
            {
                RedoStack.Clear();
            }
        }

        public static void Record(ICommand command, bool clearRedo = true)
        {
            command.Execute();

            if (clearRedo)
            {
                RedoStack.Clear();
            }

            if (UndoStack.Count + 1 >= MaxActions)
            {
                UndoStack.RemoveFirst();
            }

            UndoStack.AddLast(command);

            if (editorWindow != null)
            {
                EditorUtility.SetDirty(editorWindow);
            }
        }

        public static void PerformUndo()
        {
            if (UndoStack != null && UndoStack.Count > 0)
            {
                ICommand undoAction = UndoStack.Last.Value;
                UndoStack.RemoveLast();
                RedoStack.AddLast(undoAction);

                editorWindow.SetPage(undoAction.Page);
                undoAction.UnExecute();
            }
        }

        public static void PerformRedo()
        {
            if (RedoStack != null && RedoStack.Count > 0)
            {
                ICommand redoAction = RedoStack.Last.Value;
                RedoStack.RemoveLast();
                UndoStack.AddLast(redoAction);

                editorWindow.SetPage(redoAction.Page);
                redoAction.Execute();
            }
        }
    }
}
