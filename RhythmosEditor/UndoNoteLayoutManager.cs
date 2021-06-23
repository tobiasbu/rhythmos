using RhythmosEngine;
using System.Collections.Generic;

namespace RhythmosEditor
{
    internal struct UndoAudioReference
    {
        public int index;
        public string undoAction;
        public AudioReference note;
    }

    internal class UndoAudioReferenceManager
    {
        int maxActions = 50;
        private static List<UndoAudioReference> undoList;
        private static List<UndoAudioReference> redoList;

      

        public int UndoCount
        {
            get { return undoList.Count; }
        }

        public int RedoCount
        {
            get { return redoList.Count; }
        }

        public UndoAudioReferenceManager()
        {
            if (undoList == null)
                undoList = new List<UndoAudioReference>();

            if (redoList == null)
                redoList = new List<UndoAudioReference>();
        }

        public void Clear()
        {
            if (undoList == null)
                undoList = new List<UndoAudioReference>();
            else
                undoList.Clear();

            if (redoList == null)
                redoList = new List<UndoAudioReference>();
            else
                redoList.Clear();
        }

        public void RecordNote(AudioReference note, string action, int index, bool newThing)
        {
            UndoAudioReference undo = new UndoAudioReference();
            undo.index = index;
            undo.note = new AudioReference(note);
            undo.undoAction = action;

            if (newThing)
                redoList.Clear();

            undoList.Add(undo);
            if (undoList.Count > maxActions)
            {
                undoList.RemoveAt(0);
            }
        }

        public void RecordNoteRedo(AudioReference note, string action, int index)
        {
            UndoAudioReference redo = new UndoAudioReference();
            redo.index = index;
            redo.note = new AudioReference(note);
            redo.undoAction = action;

            redoList.Add(redo);
            if (redoList.Count > maxActions)
            {
                redoList.RemoveAt(0);
            }
        }

        public void RemoveLastUndo()
        {
            if (undoList.Count != 0)
                undoList.RemoveAt(undoList.Count - 1);
        }

        public void RemoveLastRedo()
        {
            if (redoList.Count != 0)
                redoList.RemoveAt(redoList.Count - 1);
        }

        public UndoAudioReference Undo()
        {
            if (undoList.Count != 0)
            {
                UndoAudioReference undo = undoList[undoList.Count - 1];
                return undo;
            }
            else
            {
                UndoAudioReference undo = new UndoAudioReference();
                undo.index = -1;
                undo.undoAction = "none";
                return undo;
            }
        }

        public UndoAudioReference Redo()
        {
            if (redoList.Count != 0)
            {
                UndoAudioReference redo = redoList[redoList.Count - 1];
                return redo;
            }
            else
            {
                UndoAudioReference redo = new UndoAudioReference();
                redo.index = -1;
                redo.undoAction = "none";
                return redo;
            }
        }

        public void OnUndo()
        {
            throw new System.NotImplementedException();
        }

        public void OnRedo()
        {
            throw new System.NotImplementedException();
        }
    }
}
