using System;
using System.Collections.Generic;
using UnityEngine;
using RhythmosEngine;
using RhythmosEditor.UIComponents;
using RhythmosEditor.Settings;

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

        internal Player player;

        public bool HasSelection {
            get {
                return rhythmListView.HasSelection;
            }
        }

        public bool HasNoteSelected {
            get {
                return SelectedNoteIndex >= 0;
            }
        }

        public string Name {
            get {
                return Rhythm != null ? Rhythm.Name : "";
            }
        }

        public float PlayBpm { set; get; }

        public float Bpm {
            get {
                if (Rhythm == null)
                {
                    return 0f;
                }
                if (!player.IsPlaying)
                {
                    return Rhythm.BPM;
                }
                return PlayBpm;
            }
        }

        public float BpmInSeconds {
            get { return Bpm > 0 ? 60f / Bpm : 0; }
        }

        public int NoteCount {
            get {
                return Rhythm != null ? Rhythm.Count : 0;
            }
        }

        public float CurrentNoteDuration {
            get {
                return SelectedNote != null ? SelectedNote.duration : 0;
            }
        }

        public bool CurrentNoteIsRest {
            get {
                return SelectedNote != null && SelectedNote.isRest;
            }
        }

        public List<AudioReference> AudioReferences {
            get {
                return audioReferences;
            }
        }

        internal Action<Rhythm, int> OnNoteSelect;

        public Color CurrentNoteAudioColor {
            get {
                if (SelectedNote != null)
                {
                    AudioReference audioReference = audioReferences[SelectedNote.layoutIndex];
                    if (audioReference != null)
                    {
                        return audioReference.Color;
                    }
                }
                return new Color(-1, -1, -1, -1);
            }
        }

        public bool HasAudioClips {
            get {
                return audioReferences != null && audioReferences.Count > 0;
            }
        }

        public float Zoom { get; set; } = 1f;
        public AudioClip MetronomeAudioClip { get; private set; }

        internal void Setup(ListView<Rhythm> rhythmList, RhythmosConfig config)
        {
            rhythmListView = rhythmList;
            audioReferences = config.loaded ? config.RhythmosDatabase.AudioReferences : null;
            MetronomeAudioClip = config.metroAudioClip;
            UnSelectNote();
            Rhythm = null;
            Index = -1;
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
                noteIndex = Mathf.Clamp(noteIndex, 0, current.Count - 1);
                SelectedNoteIndex = noteIndex;
                SelectedNote = current.Notes[noteIndex];
                OnNoteSelect?.Invoke(current, noteIndex);
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

        internal AudioReference GetCurrentAudioReference()
        {
            if (!HasNoteSelected)
            {
                return null;
            }

            int layoutIndex = SelectedNote.layoutIndex;

            return GetAudioReference(layoutIndex);
        }

        internal int GetLayoutIndex(AudioReference item)
        {
            if (item == null)
            {
                return -1;
            }

            if (audioReferences.Count == 0)
            {
                return -1;
            }

            return audioReferences.IndexOf(item);
        }

    }
}
