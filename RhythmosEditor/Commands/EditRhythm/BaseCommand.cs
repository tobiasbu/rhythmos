using RhythmosEditor.Pages.Rhythms;
using RhythmosEngine;


namespace RhythmosEditor.Commands.EditRhythm
{
    internal abstract class BaseCommand : ICommand
    {
        protected readonly EditController controller;
        protected int rhyhtmIndex;
        protected Rhythm rhythm;

        public int Page => 0;
        public abstract string Name { get; }
        public abstract void Execute();
        public abstract void UnExecute();

        public BaseCommand(EditController controller)
        {
            this.controller = controller;
            rhyhtmIndex = controller.Index;
            rhythm = controller.Rhythm;
        }
    }
}
