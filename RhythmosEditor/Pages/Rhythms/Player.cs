using UnityEngine;
using UnityEditor;
using RhythmosEditor.Utils;
using RhythmosEngine;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class Player
    {


        private float oldPlayHeadPosition;
        private float duration = -1;

        private float playhead;
        private bool isPlaying;

        private double lastTime;
        private double timer;
        private double nextPlayNoteTime;

        internal EditController controller;


        public float Playhead {
            get {
                return playhead;
            }
            set {
                if (isPlaying && playhead != value)
                {
                    timer = value * Duration;
                }
                playhead = value;
            }
        }
        public bool IsMovingPlayhead { get; internal set; }
        public bool Loop { get; set; } = false;
        public bool Mute { get; set; } = false;
        public bool Metronome { get; set; } = false;

        public double NoteHightlightTime { get; private set; }

        public int IndexNote { get; private set; }

        public double Timer {
            get {
                return timer;
            }
        }

        public bool IsPlaying {
            get {
                return controller.Rhythm == null ? false : isPlaying;
            }
        }

        public float Duration {
            get {
                if (controller.Rhythm == null)
                {
                    return 0;
                }

                if (duration == -1)
                {
                    duration = controller.Rhythm.Duration();
                }

                return duration;

            }
            set {
                duration = value;
            }
        }

        public Player()
        {
            isPlaying = false;
        }

        private (int, float) GetPlayheadNoteIndex()
        {
            Rhythm rhythm = controller.Rhythm;
            float playheadInSeconds = Playhead * Duration;
            if (playheadInSeconds >= Duration)
            {
                return (rhythm.Count, 0);
            }


            float sum = 0;
            float bpmInSeconds = 60f / controller.Bpm;
            for (int i = 0; i < rhythm.Count; i += 1)
            {
                sum += rhythm.Notes[i].duration * bpmInSeconds;

                if (Mathf.Approximately(playheadInSeconds, sum))
                    return (i + 1, 0);
                if (playheadInSeconds < sum)
                {
                    return (i + 1, sum - playheadInSeconds);
                }

            }
            return (0, 0);
        }

        public void Play()
        {
            if (!isPlaying)
            {
                oldPlayHeadPosition = Playhead;
            }

            lastTime = EditorApplication.timeSinceStartup;
            timer = Playhead * Duration;

            if (Playhead > 0)
            {
                var tuple = GetPlayheadNoteIndex();
                IndexNote = tuple.Item1;
                nextPlayNoteTime = timer + tuple.Item2;

                if (nextPlayNoteTime >= Duration)
                {
                    nextPlayNoteTime = Duration;
                }
            }
            else
            {
                IndexNote = 0;
                nextPlayNoteTime = 0;
            }

            isPlaying = true;
            EditorAudioUtility.StopAllClips();
        }

        public void Stop(bool stopAudios = true)
        {
            IndexNote = -1;
            isPlaying = false;
            Playhead = (oldPlayHeadPosition != -1) ? oldPlayHeadPosition : 0;
            oldPlayHeadPosition = -1;
            if (stopAudios)
            {
                EditorAudioUtility.StopAllClips();
            }
        }

        internal void Update()
        {

            if (IsMovingPlayhead)
            {
                return;
            }

            if (IsPlaying)
            {

                Playhead = Mathf.Clamp((float)timer / Duration, 0f, 1f);

                double current = EditorApplication.timeSinceStartup;
                double delta = current - lastTime;

                NoteHightlightTime -= delta;
                timer += delta;


                if (timer >= nextPlayNoteTime)
                {
                    Rhythm rhythm = controller.Rhythm;

                    if (IndexNote >= rhythm.Count)
                    {
                        if (Loop)
                        {

                            Playhead = 0;
                            IndexNote = 0;
                            timer = 0;
                            nextPlayNoteTime = 0;

                        }
                        else
                        {
                            Stop(false);
                        }
                    }
                    else
                    {

                        Note currentNote = rhythm.Notes[IndexNote];
                        if (currentNote != null)
                        {
                            float noteDuration = (60f / controller.Bpm) * rhythm.Notes[IndexNote].duration;
                            NoteHightlightTime = noteDuration;
                            if (!currentNote.isRest)
                            {
                                AudioReference audioReference = controller.GetAudioReference(currentNote.layoutIndex);
                                if (audioReference != null)
                                {
                                    if (audioReference.Clip != null)
                                    {
                                        if (!Mute)
                                        {
                                            EditorAudioUtility.PlayClip(audioReference.Clip);
                                        }
                                        NoteHightlightTime = audioReference.Clip.length;
                                    }
                                }
                            }


                            nextPlayNoteTime = timer + noteDuration;
                            IndexNote += 1;
                        }
                    }
                }
                lastTime = current;
            }
        }


        internal void SetPlayhead(float playheadPos)
        {
            Playhead = playheadPos;
            if (isPlaying)
            {
                Play();
            }
            IsMovingPlayhead = false;
        }


        internal void SetPlayheadToNote(int selectedNote)
        {
            Rhythm rhythm = controller.Rhythm;
            if (rhythm == null)
            {
                return;
            }

            if (rhythm.Count == 0)
            {
                return;
            }

            if (selectedNote >= 0 && selectedNote < rhythm.Count)
            {
                float sum = 0;
                for (int i = 0; i < selectedNote; i += 1)
                {
                    sum += rhythm.Notes[i].duration;
                }


                SetPlayhead((sum * 60f / controller.Bpm) / Duration);
            }
        }



    }
}
