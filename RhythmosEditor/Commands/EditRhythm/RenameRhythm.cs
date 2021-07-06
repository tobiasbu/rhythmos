using RhythmosEditor.Pages.Rhythms;
using UnityEngine;

namespace RhythmosEditor.Commands.EditRhythm
{
    internal class RenameRhythm : BaseCommand
    {
        private readonly string newName;
        private readonly string oldName;

        public override string Name => "Set Rhythm name";

        public RenameRhythm(EditController controller, string name) : base(controller)
        {
            newName = name;
            oldName = rhythm.Name;
        }

        public override void Execute()
        {
            rhythm.Name = newName;
            controller.SelectRhythm(rhyhtmIndex);
            GUI.FocusControl("Rhythm name");
        }

        public override void UnExecute()
        {
            rhythm.Name = oldName;
            controller.SelectRhythm(rhyhtmIndex);
            GUI.FocusControl("Rhythm name");
        }
    }
}
