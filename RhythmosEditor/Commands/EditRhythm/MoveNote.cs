using RhythmosEditor.Pages.Rhythms;
using UnityEngine;

namespace RhythmosEditor.Commands.EditRhythm
{
    internal class MoveNote : BaseNoteCommand
    {
       
        public override string Name => "Move note to " + moveDirection.ToString();

        public readonly Direction moveDirection;
        private int moveIndex;

        public MoveNote(EditController controller, Direction direction) : base(controller)
        {
            moveDirection = direction;
            note = controller.SelectedNote;
            noteIndex = controller.SelectedNoteIndex;
        }

        public override void Execute()
        {
            if (moveDirection == Direction.Right)
            {

                moveIndex = noteIndex + 1;
            }
            else
            {
                moveIndex = noteIndex - 1;
            }

            moveIndex = Mathf.Clamp(moveIndex, 0, rhythm.Count);
            rhythm.SwapNote(noteIndex, moveIndex);

            if (rhythm.Count > 0)
            {
                controller.SelectNote(moveIndex, rhyhtmIndex);
            }
            else
            {
                controller.UnSelectNote();
            }
        }

        public override void UnExecute()
        {
            rhythm.SwapNote(moveIndex, noteIndex);

            if (rhythm.Count > 0)
            {
                controller.SelectNote(noteIndex, rhyhtmIndex);
            }
            else
            {
                controller.UnSelectNote();
            }
        }
    }
}
