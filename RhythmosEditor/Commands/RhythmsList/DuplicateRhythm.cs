using RhythmosEngine;
using RhythmosEditor.UIComponents;

namespace RhythmosEditor.Commands.RhythmsList
{
    using RhythmListView = ListView<Rhythm>;

    internal class DuplicateRhythm : BaseCommand
    {
        public override string Name => "Duplicate " + (rhythm != null ? rhythm.Name : "Rhythm");

        public DuplicateRhythm(RhythmListView list)
            : base(list, null, -1)
        { }

        public override void Execute()
        {
            if (index == -1)
            {
                Rhythm clone = new Rhythm(rhythmListView.Current);
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
