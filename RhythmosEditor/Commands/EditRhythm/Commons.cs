using RhythmosEditor.Pages.Rhythms;
using RhythmosEngine;
using UnityEngine;

namespace RhythmosEditor.Commands.EditRhythm
{
    internal static class Commons
    {
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
