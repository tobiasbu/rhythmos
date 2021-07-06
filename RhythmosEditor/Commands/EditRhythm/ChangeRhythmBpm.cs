using RhythmosEditor.Pages.Rhythms;
using UnityEngine;

namespace RhythmosEditor.Commands.EditRhythm
{
    internal class ChangeRhythmBpm : BaseCommand
    {
        private readonly float newBpm;
        private readonly float oldBpm;

        public override string Name => "Change rhythm BPM";

        public ChangeRhythmBpm(EditController controller, float bpm, float oldBpm) : base(controller)
        {
            newBpm = bpm;
            this.oldBpm = oldBpm;
        }

        public override void Execute()
        {
            rhythm.BPM = newBpm;
            controller.SelectRhythm(rhyhtmIndex);
            GUI.FocusControl("Tempo");
        }

        public override void UnExecute()
        {
            rhythm.BPM = oldBpm;
            controller.SelectRhythm(rhyhtmIndex);
            GUI.FocusControl("Tempo");
        }
    }
}