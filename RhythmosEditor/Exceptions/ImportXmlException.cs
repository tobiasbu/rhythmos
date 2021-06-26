using System;

namespace RhythmosEditor.Exceptions
{
    public class ImportXmlException : Exception
    {
        public ImportXmlException(string message)
            : base(string.Concat("RhythmosEditor: ", message))
        { }
        public ImportXmlException(string message, Exception innerException)
            : base(string.Concat("RhythmosEditor: ", message), innerException)
        { }

    }
}
