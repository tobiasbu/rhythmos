using RhythmosEngine;
using RhythmosEditor.UIComponents;

namespace RhythmosEditor.Commands.RhythmsList
{
    using RhythmListView = ListView<Rhythm>;

    internal class DeleteRhythm : BaseCommand
    {
        public override string Name => "Remove " + (rhythm != null ? rhythm.Name : "Rhythm");

        public DeleteRhythm(RhythmListView list)
            : base(list, list.Current, list.SelectedIndex)
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
