using RhythmosEngine;
using RhythmosEditor.UIComponents;

namespace RhythmosEditor.Commands.RhythmsList
{
    using RhythmListView = ListView<Rhythm>;

    internal class Create : BaseCommand
    {
        public override string Name => "Create " + (rhythm != null ? rhythm.Name : "Rhythm");

        public Create(RhythmListView list)
            : base(list, null, -1)
        { }

        public override void Execute()
        {
            int count = rhythmListView.List.Count;
            if (index == -1)
            {
                Rhythm newRhythm = new Rhythm
                {
                    Name = "New Rhythm " + (count + 1),
                    BPM = 80
                };
                rhythmListView.List.Add(newRhythm);

                index = count;
                rhythm = newRhythm;

                rhythmListView.Select(count);
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