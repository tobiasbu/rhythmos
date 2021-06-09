using System;
using UnityEngine;

namespace RhythmosEngine
{
    /// <summary>
    /// Representation of a Note Layout.
    /// </summary>
    [Serializable]
    public class NoteLayout
    {
        /// <summary>
        /// Note name.
        /// </summary>
        public string Name = "";

        /// <summary>
        /// Note audio clip.
        /// </summary>
        public AudioClip Clip = null;

        /// <summary>
        /// Note color (used in RhythmosEditor)
        /// </summary>
        public Color Color = new Color(UnityEngine.Random.Range(0.3f, 0.95f), UnityEngine.Random.Range(0.3f, 0.95f), UnityEngine.Random.Range(0.3f, 0.95f));

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.NoteLayout"/> class.
        /// </summary>
        public NoteLayout()
        {
            Name = "";
            Clip = null;
            Color = new Color(UnityEngine.Random.Range(0.3f, 0.95f), UnityEngine.Random.Range(0.3f, 0.95f), UnityEngine.Random.Range(0.3f, 0.95f));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.NoteLayout"/>.
        /// Audio clip will be null and color is randomized.
        /// </summary>
        /// <param name="name">Note layout name</param>
        public NoteLayout(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.NoteLayout"/>.
        /// Color is randomized.
        /// </summary>
        /// <param name="name">Note layout Name.</param>
        /// <param name="clip">Note layout AudioClip.</param>
        public NoteLayout(string name, AudioClip clip)
        {
            Name = name;
            Clip = clip;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.NoteLayout"/>.
        /// </summary>
        /// <param name="name">Note layout Name.</param>
        /// <param name="clip">Note layout Clip.</param>
        /// <param name="color">Note layout Color.</param>
        public NoteLayout(string name, AudioClip clip, Color color)
        {
            Name = name;
            Clip = clip;
            Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.NoteLayout"/> based in another NoteLayout.
        /// </summary>
        /// <param name="noteLayout">Note layout.</param>
        public NoteLayout(NoteLayout noteLayout)
        {
            Name = noteLayout.Name;
            Color = noteLayout.Color;
            Clip = noteLayout.Clip;
        }
    }
}

