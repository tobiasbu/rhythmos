using RhythmosEditor.Pages.Rhythms;

namespace RhythmosEditor.Commands.EditRhythm
{
    internal class RemoveNote : BaseNoteCommand
    {
        public RemoveNote(EditController controller) : base(controller)
        {
            note = controller.SelectedNote;
            noteIndex = controller.SelectedNoteIndex;
        }

        public override string Name => "Remove note";

        public override void Execute()
        {
            Commons.RemoveNoteAt(controller, rhyhtmIndex, noteIndex);
        }

        public override void UnExecute()
        {
            Commons.AddNoteAt(controller, rhyhtmIndex, note, noteIndex);
        }
    }
}
