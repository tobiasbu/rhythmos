using System.Collections.Generic;
using UnityEngine;
using RhythmosEngine;
using RhythmosEditor.UIComponents;
using RhythmosEditor.Pages.Rhythms;

namespace RhythmosEditor.Commands
{
    using RhythmListView = ListView<Rhythm>;

    internal enum Direction
    {
        Left,
        Right
    }


    internal static class Commons
    {
        public static void AddRhythmAt(RhythmListView rhythmListView, Rhythm rhythm, int index)
        {
            IList<Rhythm> list = rhythmListView.List;
            int insertAt = Mathf.Clamp(index, 0, list.Count);

            if (list.Count > 0)
            {
                list.Insert(insertAt, rhythm);
            }
            else
            {
                list.Add(rhythm);
            }
            rhythmListView.Select(insertAt);
        }

        public static void RemoveRhythmAt(RhythmListView rhythmListView, Rhythm rhythm, int index)
        {
            IList<Rhythm> list = rhythmListView.List;
            list.Remove(rhythm);

            if (list.Count > 0)
            {
                int selectAt = Mathf.Clamp(index, 0, list.Count - 1);
                rhythmListView.Select(selectAt);
            }
            else
            {
                rhythmListView.UnSelect();
            }
        }

        public static int AddNoteAt(EditController controller, int rhythmIndex, Note note, int noteIndex)
        {
            Rhythm rhythm = controller.GetRhythmAt(rhythmIndex);
            int insertAt = Mathf.Clamp(noteIndex, 0, rhythm.Count);

            if (rhythm.Count > 0)
            {
                rhythm.InsertNoteAt(insertAt, note);
            }
            else
            {
                rhythm.AppendNote(note);
            }
            controller.SelectNote(insertAt, rhythmIndex);
            return insertAt;
        }

        internal static void RemoveNoteAt(EditController controller, int rhythmIndex, int noteIndex)
        {
            Rhythm rhythm = controller.GetRhythmAt(rhythmIndex);
            rhythm.RemoveNote(noteIndex);

            if (rhythm.Count > 0)
            {
                int selectAt = Mathf.Clamp(noteIndex, 0, rhythm.Count - 1);
                controller.SelectNote(selectAt, rhythmIndex);
            }
            else
            {
                controller.UnSelectNote();
            }
        }

        
    }
}
