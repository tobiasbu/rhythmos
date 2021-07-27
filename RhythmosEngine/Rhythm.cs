using System;
using System.Collections.Generic;
using UnityEngine;

namespace RhythmosEngine
{
    /// <summary>
    /// Represents an musical rhythm.
    /// </summary>
    [Serializable]
    public class Rhythm
    {
        [SerializeField]
        private List<Note> m_noteList;

        /// <summary>
        /// Rhythm name.
        /// </summary>
        public string Name;

        /// <summary>
        /// Beats per minute.
        /// </summary>
        public float BPM;

        /// <summary>
        /// Gets the note count.
        /// </summary>
        /// <value>The note count.</value>
        [Obsolete("Please use 'Count' property instead. Just to simplify things.", true)]
        public int NoteCount
        {
            get
            {
                return m_noteList.Count;
            }
        }

        /// <summary>
        /// Gets the note count.
        /// </summary>
        public int Count
        {
            get
            {
                return m_noteList == null ? 0 : m_noteList.Count;
            }
        }

        /// <summary>
        /// Get notes list of this rhythm.
        /// </summary>
        public List<Note> Notes
        {
            get
            {
                return m_noteList;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.Rhythm"/> class.
        /// </summary>
        public Rhythm()
        {
            Name = "none";
            BPM = 80;
            m_noteList = new List<Note>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.Rhythm"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="bpm">BPM.</param>
        public Rhythm(string name, float bpm)
        {
            Name = name;
            BPM = bpm;
            m_noteList = new List<Note>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.Rhythm"/> class cloning another Rhythm instance.
        /// </summary>
        /// <param name="rhythm">Rhythm.</param>
        public Rhythm(Rhythm rhythm)
        {
            Name = rhythm.Name;
            BPM = rhythm.BPM;
            m_noteList = new List<Note>();

            foreach (Note nt in rhythm.Notes)
            {
                m_noteList.Add(new Note(nt));
            }
        }

        /// <summary>
        /// Appends a note to Rhythm.
        /// </summary>
        /// <param name="note">Note.</param>
        public void AppendNote(Note note)
        {
            m_noteList.Add(note);
        }

        /// <summary>
        /// Appends a note.
        /// </summary>
        /// <param name="layoutIndex">Layout index.</param>
        /// <param name="noteDuration">Note_duration.</param>
        /// <param name="isRest">If set to <c>true</c> is_pause.</param>
        public void AppendNote(int layoutIndex, float noteDuration, bool isRest)
        {
            Note newNote = new Note();
            newNote.isRest = isRest;
            newNote.duration = noteDuration;
            newNote.layoutIndex = layoutIndex;
            m_noteList.Add(newNote);
        }

        /// <summary>
        /// Inserts a note at index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="note">Note.</param>
        public void InsertNoteAt(int index, Note note)
        {
            m_noteList.Insert(index, note);
        }

        /// <summary>
        /// Removes a note at index.
        /// </summary>
        /// <param name="index">Index.</param>
        public void RemoveNote(int index)
        {
            m_noteList.RemoveAt(index);
        }

        /// <summary>
        /// Replaces a note at index.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="note">Note.</param>
        public void ReplaceNote(int index, Note note)
        {
            m_noteList.RemoveAt(index);
            m_noteList.Insert(index, note);
        }

        /// <summary>
        /// Swaps a note in the index to swapIndex.
        /// </summary>
        /// <param name="index">Index.</param>
        /// <param name="swapIndex">Swap index.</param>
        public void SwapNote(int index, int swapIndex)
        {
            Note note = new Note(m_noteList[index]);
            Note noteSwap = new Note(m_noteList[swapIndex]);

            m_noteList.RemoveAt(index);
            m_noteList.Insert(index, noteSwap);

            m_noteList.RemoveAt(swapIndex);
            m_noteList.Insert(swapIndex, note);

        }

        /// <summary>
        /// Clear the note list.
        /// </summary>
        public void Clear()
        {
            m_noteList.Clear();
        }


        /// <summary>
        /// Duration of Rhythm by BPM.
        /// </summary>
        public float Duration()
        {
            float totalTime = 0f;
            if (BPM > 0)
            {
                foreach (Note nt in m_noteList)
                {
                    totalTime += nt.duration;
                }
                totalTime *= 60f / BPM;
            }

            return totalTime;
        }

        /// <summary>
        /// Notes the list.
        /// </summary>
        /// <returns>Note list.</returns>
        [Obsolete("Please use 'Notes' property instead. Just to simplify things.", true)]
        public List<Note> NoteList()
        {
            return m_noteList;
        }

        /// <summary>
        /// Gets the note at index.
        /// </summary>
        /// <returns>The <see cref="RhythmosEngine.Note"/>.</returns>
        /// <param name="index">Index.</param>
        public Note GetNoteAt(int index)
        {
            return m_noteList[index];
        }

    };
}

