using System;

namespace RhythmosEngine
{
    /// <summary>
    /// Represents an exception for database loader <see cref="RhythmosEngine.RhythmosDatabase"/>
    /// </summary>
    public class DatabaseLoadException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public DatabaseLoadException(string message)
            : base(string.Concat("RhythmosEngine: ", message)) 
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DatabaseLoadException(string message, Exception innerException)
            : base(string.Concat("RhythmosEngine: ", message), innerException)
        { }

    }

}
