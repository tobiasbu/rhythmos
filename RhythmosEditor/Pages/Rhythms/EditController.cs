using UnityEngine;
using RhythmosEngine;
using RhythmosEditor.UIComponents;
using System.Collections.Generic;
using System;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class EditController
    {
        private ListView<Rhythm> rhythmListView;

        private List<AudioReference> audioReferences;

        public Rhythm Rhythm { get; private set; }
        public int Index { get; private set; }

        public Note SelectedNote { get; private set; }
        public int SelectedNoteIndex { get; private set; } = -1;

        public bool HasSelection
        { 
            get
            {
                return rhythmListView.HasSelection;
            } 
        }

        public bool HasNoteSelected
        {
            get
            {
                return SelectedNoteIndex >= 0;
            }
        }

        public string Name 
        { 
            get
            {
                return Rhythm != null ? Rhythm.Name : "";
            }
        }


        public float BPM
        {
            get
            {
                return Rhythm != null ? Rhythm.BPM : 0;
            }
        }
        internal void Setup(ListView<Rhythm> rhythmList, List<AudioReference> audioReferencesList)
        {
            rhythmListView = rhythmList;
            audioReferences = audioReferencesList;
        }


        internal Rhythm GetRhythmAt(int rhythmIndex)
        {
            return rhythmListView.List[rhythmIndex];
        }

        internal void SetRhythm(Rhythm rhythm, int index)
        {
            Rhythm = rhythm;
            Index = index;
            SelectedNoteIndex = -1;
            SelectedNote = null;
        }

        internal void SelectRhythm(int index)
        {
            rhythmListView.Select(index);
            if (index != Index)
            {
                SelectedNoteIndex = -1;
                SelectedNote = null;
            }
        }

        internal void SelectNote(int noteIndex, int rhythmIndex)
        {
            SelectRhythm(rhythmIndex);
            if (rhythmListView.HasSelection)
            {
                Rhythm current = rhythmListView.Current;

                if (current.Count == 0)
                {
                    return;
                }
                noteIndex = Mathf.Clamp(noteIndex, 0, current.Count);
                SelectedNoteIndex = noteIndex;
                SelectedNote = current.Notes[noteIndex];
            }
        }

        internal void SelectNote(int noteIndex)
        {
            SelectNote(noteIndex, rhythmListView.SelectedIndex); 
        }

        internal void UnSelectNote()
        {
            SelectedNoteIndex = -1;
            SelectedNote = null;
        }

        internal AudioReference GetAudioReference(int layoutIndex)
        {
            if (audioReferences.Count == 0)
            {
                return null;
            }

            if (layoutIndex < 0 || layoutIndex >= audioReferences.Count)
            {
                return null;
            }

            return audioReferences[layoutIndex];
        }
    }
}
