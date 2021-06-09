using System;

namespace RhythmosEngine
{
    public class DatabaseLoadException : Exception
    {
        public DatabaseLoadException(string message)
            : base(message) 
        { }
        public DatabaseLoadException(string message, Exception innerException)
            : base(string.Concat("RhythmosEngine: ", message), innerException)
        { }

    }

}
