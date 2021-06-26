using System;

namespace RhythmosEditor.Exceptions
{
    public class ExportXmlException : Exception
    {
        public ExportXmlException(string message)
            : base(string.Concat("RhythmosEditor: ", message))
        { }
        public ExportXmlException(string message, Exception innerException)
            : base(string.Concat("RhythmosEditor: ", message), innerException)
        { }

    }
}
