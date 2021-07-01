using RhythmosEngine;
using RhythmosEditor.Pages.Rhythms;

namespace RhythmosEditor.Commands.EditRhythm
{
    internal abstract class BaseNoteCommand : BaseCommand
    {
        protected int noteIndex;
        protected int selectedNoteIndex;
        protected Note note;

        protected BaseNoteCommand(EditController controller) : base(controller)
        {
            selectedNoteIndex = controller.SelectedNoteIndex;
            rhythm = controller.Rhythm;
            rhyhtmIndex = controller.Index;
            note = null;
            noteIndex = -1;
        }
    }
}
