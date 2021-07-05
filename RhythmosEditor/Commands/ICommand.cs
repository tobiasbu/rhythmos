namespace RhythmosEditor.Commands
{
    internal interface ICommand
    {
        string Name { get; }
        int Page { get; }

        void Execute();
        void UnExecute();
    }
}
