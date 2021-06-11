using System;


namespace RhythmosEditor
{
    public class ExportException : Exception
    {
        public ExportException(string message)
            : base(string.Concat("RhythmosEditor: ", message))
        { }
        public ExportException(string message, Exception innerException)
            : base(string.Concat("RhythmosEditor: ", message), innerException)
        { }

    }

    public class ImportException : Exception
    {
        public ImportException(string message)
            : base(string.Concat("RhythmosEditor: ", message))
        { }
        public ImportException(string message, Exception innerException)
            : base(string.Concat("RhythmosEditor: ", message), innerException)
        { }

    }
}
