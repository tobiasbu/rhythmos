using RhythmosEngine;
using System.Collections.Generic;

namespace RhythmosEditor
{
    internal struct UndoRhythm
    {
        public int index;
        public string undoAction;
        public Rhythm rhythm;
        public int lastSelectedNote;
    }

    internal class UndoRhythmManager
    {
        int maxActions = 50;

        private static List<UndoRhythm> undoList;
        private static List<UndoRhythm> redoList;
        public int UndoCount
        {
            get { return undoList.Count; }
        }

        public int RedoCount
        {
            get { return redoList.Count; }
        }

        public UndoRhythmManager()
        {
            if (undoList == null)
                undoList = new List<UndoRhythm>();

            if (redoList == null)
                redoList = new List<UndoRhythm>();
        }

        public void Clear()
        {

            if (undoList == null)
                undoList = new List<UndoRhythm>();
            else
                undoList.Clear();

            if (redoList == null)
                redoList = new List<UndoRhythm>();
            else
                redoList.Clear();

        }

        public void RecordRhythm(Rhythm rhythm, string action, int index, int lastNote, bool newThing)
        {

            UndoRhythm undo = new UndoRhythm();

            undo.index = index;
            undo.rhythm = new Rhythm(rhythm);
            undo.undoAction = action;
            undo.lastSelectedNote = lastNote;

            undoList.Add(undo);

            if (newThing)
                redoList.Clear();

            if (undoList.Count > maxActions)
            {
                undoList.RemoveAt(0);
            }
        }

        public void RecordRhythmRedo(Rhythm rhythm, string action, int index, int lastNote)
        {

            UndoRhythm redo = new UndoRhythm();

            redo.index = index;
            redo.rhythm = new Rhythm(rhythm);
            redo.undoAction = action;
            redo.lastSelectedNote = lastNote;

            redoList.Add(redo);

            if (redoList.Count > maxActions)
            {
                redoList.RemoveAt(0);
            }

        }

        public void RemoveLastUndo()
        {
            if (undoList.Count != 0)
            {
                undoList.RemoveAt(undoList.Count - 1);
            }
        }

        public void RemoveLastRedo()
        {
            if (redoList.Count != 0)
            {
                redoList.RemoveAt(redoList.Count - 1);
            }
        }

        public UndoRhythm Undo()
        {
            if (undoList.Count != 0)
            {
                UndoRhythm undo = undoList[undoList.Count - 1];
                return undo;
            }
            else
            {
                UndoRhythm undo = new UndoRhythm();
                undo.index = -1;
                undo.undoAction = "none";
                undo.rhythm = null;
                return undo;
            }
        }

        public UndoRhythm Redo()
        {
            if (redoList.Count != 0)
            {
                UndoRhythm redo = redoList[redoList.Count - 1];
                return redo;
            }
            else
            {
                UndoRhythm redo = new UndoRhythm();
                redo.index = -1;
                redo.undoAction = "none";
                redo.rhythm = null;
                return redo;
            }
        }
    }
}
