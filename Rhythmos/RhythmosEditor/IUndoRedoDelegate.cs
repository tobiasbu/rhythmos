namespace RhythmosEditor
{
    interface IUndoRedoDelegate
    {
        int UndoCount { get; }
        int RedoCount { get; }
        void OnUndo();
        void OnRedo();
    }
}
