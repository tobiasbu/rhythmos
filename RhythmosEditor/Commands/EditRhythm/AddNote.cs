using RhythmosEditor.Pages.Rhythms;
using RhythmosEngine;

namespace RhythmosEditor.Commands.EditRhythm
{
    internal class AddNote : BaseNoteCommand
    {
        protected float beatValue;
  
        public override string Name => "Add rhythm note";

        public AddNote(EditController controller, float noteValue = 0.5f) : base(controller)
        {
            beatValue = noteValue;
        }

        public override void Execute()
        {
            if (noteIndex == -1)
            {
                note = new Note() { duration = beatValue, isRest = false, layoutIndex = 0 };
                int insertAt;
                if (selectedNoteIndex >= 0)
                {
                    insertAt = selectedNoteIndex + 1;
                }
                else
                {
                    insertAt = rhythm.Count;
                }
                noteIndex = insertAt;
            }

            Commons.AddNoteAt(controller, rhyhtmIndex, note, noteIndex);
        }

        public override void UnExecute()
        {
            Commons.RemoveNoteAt(controller, rhyhtmIndex, noteIndex);
        }
    }
}
