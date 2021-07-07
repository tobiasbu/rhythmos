using RhythmosEditor.Pages.Rhythms;
using RhythmosEngine;

namespace RhythmosEditor.Commands.EditRhythm
{
    internal class DuplicateNote : BaseNoteCommand
    {
        public DuplicateNote(EditController controller) : base(controller)
        {
        }

        public override string Name => "Duplicate note";

        public override void Execute()
        {
            if (noteIndex == -1)
            {
                Note clone = new Note(controller.SelectedNote);
                int insertIndex = controller.SelectedNoteIndex + 1;
               
                noteIndex = insertIndex;
                note = clone;
            }

            Commons.AddNoteAt(controller, rhyhtmIndex, note, noteIndex);
        }

        public override void UnExecute()
        {
            Commons.RemoveNoteAt(controller, rhyhtmIndex, noteIndex);
        }
    }
}
