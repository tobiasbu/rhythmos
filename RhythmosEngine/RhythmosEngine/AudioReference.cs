using System;
using UnityEngine;

namespace RhythmosEngine
{
    /// <summary>
    /// Stores <see cref="AudioClip"/> reference and related <see cref="UnityEngine.Color"/> identifier
    /// </summary>
    [Serializable]
    public class AudioReference 
    {
        /// <summary>
        /// Actual <see cref="AudioClip"/> reference
        /// </summary>
        public AudioClip Clip;

        /// <summary>
        /// Color identifier
        /// </summary>
        public Color Color;

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.AudioReference"/>
        /// </summary>
        public AudioReference()
        {
            Clip = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.AudioReference"/>
        /// </summary>
        /// <param name="clip">AudioClip</param>
        public AudioReference(AudioClip clip)
        {
            Clip = clip;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.AudioReference"/>
        /// </summary>
        /// <param name="clip">AudioClip</param>
        /// <param name="color">Color identifier</param>
        public AudioReference(AudioClip clip, Color color)
        {
            Clip = clip;
            Color = color;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="otherAudioReference">Other <see cref="RhythmosEngine.AudioReference"/> instance to copy</param>
        public AudioReference(AudioReference otherAudioReference) {
            Color = otherAudioReference.Color;
            Clip = otherAudioReference.Clip;
        }
    }
}
