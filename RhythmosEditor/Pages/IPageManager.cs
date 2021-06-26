namespace RhythmosEditor.Pages
{
    internal interface IPageManager
    {
        int Selection { get; }
        void SetPage(int pageIndex, bool force = false);
        void SetDirty();
    }
}
