using System;
using System.Collections.Generic;
using RhythmosEditor.Pages;
using RhythmosEditor.Utils;
using UnityEditor;

namespace RhythmosEditor.Commands
{
    internal static class UndoRedo
    {
        internal class DelayedRecordHolder
        {
            public Func<ICommand> command;

            public void Apply()
            {
                if (dispatcher != null && delayedRecord == this)
                {
                    dispatcher.Stop(true);
                    delayedRecord = null;
                }
            }

        }

        private static IPageManager pageManager;
        private static DelayedRecordHolder delayedRecord;
        private static DebounceDispatcher dispatcher;
        private static readonly LinkedList<ICommand> UndoStack;
        private static readonly LinkedList<ICommand> RedoStack;

        public static uint MaxActions { set; get; } = 50;

        public static int UndoCount {
            get { return UndoStack != null ? UndoStack.Count : 0; }
        }

        public static int RedoCount {
            get { return RedoStack != null ? RedoStack.Count : 0; }
        }

        static UndoRedo()
        {
            UndoStack = new LinkedList<ICommand>();
            RedoStack = new LinkedList<ICommand>();
        }

        public static void SetPageManager(IPageManager manager)
        {
            pageManager = manager;
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

        public static void Record(ICommand command)
        {
            if (delayedRecord != null)
            {
                delayedRecord.Apply();
                delayedRecord = null;
            }

            command.Execute();
            UndoStack.AddLast(command);
            RedoStack.Clear();

            if (UndoStack.Count + 1 >= MaxActions)
            {
                UndoStack.RemoveFirst();
            }

            if (pageManager != null)
            {
                pageManager.SetDirty();
            }
        }

        public static void DelayedRecord(DelayedRecordHolder record, int interval = 500)
        {
            if (record == null || record.command == null)
            {
                return;
            }

            if (delayedRecord != record)
            {
                if (delayedRecord != null)
                {
                    delayedRecord.Apply();
                    delayedRecord = null;
                }
                delayedRecord = record;
            }

            if (dispatcher == null)
            {
                dispatcher = new DebounceDispatcher();
            }

            dispatcher.Debounce(() => {
                ICommand command = delayedRecord?.command?.Invoke();
                delayedRecord = null;
                if (command != null)
                {
                    Record(command);
                }
            }, interval);

        }

        public static void PerformUndo()
        {
            if (UndoStack != null && UndoStack.Count > 0)
            {
                ICommand undoAction = UndoStack.Last.Value;
                UndoStack.RemoveLast();
                RedoStack.AddLast(undoAction);

                EditorGUIUtility.editingTextField = false;
                pageManager.SetPage(undoAction.Page);
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

                EditorGUIUtility.editingTextField = false;
                pageManager.SetPage(redoAction.Page);
                redoAction.Execute();
            }
        }
    }
}
