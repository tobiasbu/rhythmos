using RhythmosEngine;

namespace RhythmosEditor.Commands.Rhythms
{
    using RhythmListView = ListView<Rhythm>;

    internal abstract class BaseCommand : ICommand
    {
        protected readonly RhythmListView rhythmListView;
        protected int index;
        protected Rhythm rhythm;

        public int Page => 0;
        public abstract string Name { get; }
        public abstract void Execute();
        public abstract void UnExecute();

        public BaseCommand(RhythmListView listView, int index, Rhythm rhythm)
        {
            rhythmListView = listView;
            this.index = index;
            this.rhythm = rhythm;
        }
    }
}
