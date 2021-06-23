using RhythmosEngine;

namespace RhythmosEditor.Commands.Rhythms
{
    using RhythmListView = ListView<Rhythm>;

    internal class Delete : BaseCommand
    {
        public override string Name => "Remove " + (rhythm != null ? rhythm.Name : "Rhythm");

        public Delete(RhythmListView list)
            : base(list, list.SelectedIndex, list.Selected)
        {
        }

        public override void Execute()
        {
            Commons.RemoveRhythmAt(rhythmListView, rhythm, index);
        }

        public override void UnExecute()
        {
            Commons.AddRhythmAt(rhythmListView, rhythm, index);
        }
    }
}
