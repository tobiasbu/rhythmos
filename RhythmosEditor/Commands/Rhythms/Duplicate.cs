using RhythmosEngine;

namespace RhythmosEditor.Commands.Rhythms
{
    using RhythmListView = ListView<Rhythm>;

    internal class Duplicate : BaseCommand
    {
        public override string Name => "Duplicate " + (rhythm != null ? rhythm.Name : "Rhythm");

        public Duplicate(RhythmListView list)
            : base(list, -1, null)
        { }

        public override void Execute()
        {
            if (index == -1)
            {
                Rhythm clone = new Rhythm(rhythmListView.Selected);
                clone.Name = string.Concat(clone.Name, " (Copy)");

                int insertIndex = rhythmListView.SelectedIndex + 1;
                Commons.AddRhythmAt(rhythmListView, clone, insertIndex);

                index = insertIndex;
                rhythm = clone;
            }
            else
            {
                Commons.AddRhythmAt(rhythmListView, rhythm, index);
            }
        }

        public override void UnExecute()
        {
            Commons.RemoveRhythmAt(rhythmListView, rhythm, index);
        }
    }
}
