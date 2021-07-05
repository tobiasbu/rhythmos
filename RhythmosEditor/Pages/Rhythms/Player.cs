using System.Collections.Generic;
using UnityEditor;
using RhythmosEditor.Utils;
using RhythmosEngine;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class Player
    {
        private Rhythm rhythm;
        private float duration;
        private bool isPlaying;
        private float timerAmmount;
        private int indexNote;
        private double timer;


        public bool Loop { get; set; } = false;
        public bool Mute { get; set; } = false;
        public bool Metronome { get; set; } = false;

        public bool IsPlaying {
            get {
                return rhythm == null ? false : isPlaying;
            }
        }

        public float Duration { get; }
        
        public Rhythm Rhythm
        {
            get
            {
                return rhythm;
            }
        }



        public Player()
        {
            isPlaying = false;
            rhythm = null;
        }

        public void Play()
        {
            indexNote = -1;
            timer = EditorApplication.timeSinceStartup;
            isPlaying = true;
        }

        public void Stop()
        {
            indexNote = -1;
            timerAmmount = 0;
            isPlaying = false;
            AudioUtility.StopAllClips();
        }

        public void SetRhythm(Rhythm rhythm)
        {
            this.rhythm = rhythm;
            if (isPlaying)
            {
                Stop();
            }
        }

        public void OnUpdate()
        {
            if (IsPlaying)
            {

            }
        }
    }
}
