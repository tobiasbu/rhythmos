using UnityEngine;
using System.Collections.Generic;

namespace RhythmosEngine
{

    /// <summary>
    /// RhythmPlayer is component that allows you to play rhythms in your game
    /// </summary>
    public class RhythmosPlayer : MonoBehaviour
    {
        [SerializeField]
        private Rhythm m_rhythm;

        [SerializeField]
        private RhythmosDatabase m_rhythmDatabase;

        [SerializeField]
        private bool m_loop = false;

        [SerializeField]
        private float m_volume = 1f;

        [SerializeField]
        private bool m_destroyOnEnd = false;

        private float m_rhythmDuration = 0;
        private int m_noteIndex = -1;
        private float m_noteTime = 0;
        private float m_playedAmount = 0;
        private bool m_noteWasPlayed = false;
        private bool m_paused = false;
        private AudioSource m_lastAudioSource = null;
        private List<AudioSource> m_audioSourcesList = new List<AudioSource>();

        /// <summary>
        /// Defines if rhythm is being played
        /// </summary>
        protected bool m_playing = false;

        /// <summary>
        /// Gets the rhythm.
        /// </summary>
        /// <value>The rhythm.</value>
        public Rhythm rhythm
        {
            get
            {
                return m_rhythm;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RhythmosEngine.RhythmosPlayer"/> is looping.
        /// </summary>
        /// <value><c>true</c> if loop; otherwise, <c>false</c>.</value>
        public bool loop
        {
            set
            {
                m_loop = value;
            }
            get
            {
                return m_loop;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating when the played rhythm was finished of this <see cref="RhythmosEngine.RhythmosPlayer"/> will destroy or not.
        /// </summary>
        /// <value><c>true</c> if will destroy on end of played rhythm the GameObject; otherwise, <c>false</c>.</value>
        public bool destroyOnEnd
        {
            get
            {
                return m_destroyOnEnd;
            }
            set
            {
                m_destroyOnEnd = value;
            }
        }

        /// <summary>
        /// Gets or sets the rhythm database.
        /// </summary>
        /// <value>The rhythm database.</value>
        public RhythmosDatabase rhythmDatabase
        {
            set
            {
                m_rhythmDatabase = value;
            }
            get
            {
                return m_rhythmDatabase;
            }
        }

        /// <summary>
        /// Gets the actual index of playing rhythm.
        /// </summary>
        /// <value>The index of the note.</value>
        public int noteIndex
        {
            get
            {
                return m_noteIndex;
            }
        }

        /// <summary>
        /// Gets the playback position in seconds.
        /// </summary>
        /// <value>The played time.</value>
        public float time
        {
            get
            {
                return m_playedAmount;
            }
        }

        /// <summary>
        /// Gets the track position in seconds
        /// </summary>
        /// <value>The track position.</value>
        public float trackPosition
        {
            get
            {
                if (m_rhythm != null)
                    return m_playedAmount / m_rhythmDuration;
                else
                    return 0;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this <see cref="RhythmosEngine.RhythmosPlayer"/> is playing.
        /// </summary>
        /// <value><c>true</c> if is playing; otherwise, <c>false</c>.</value>
        public bool isPlaying
        {
            get
            {
                return m_playing;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="RhythmosEngine.RhythmosPlayer"/> is paused.
        /// </summary>
        /// <value><c>true</c> if is paused; otherwise, <c>false</c>.</value>
        public bool isPaused
        {
            get
            {
                return m_paused;
            }
        }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>The volume.</value>
        public float volume
        {
            get
            {
                return m_volume;
            }
            set
            {
                m_volume = value;

                if (m_volume > 1)
                {
                    m_volume = 1;
                }
                else if (m_volume < 0)
                {
                    m_volume = 0;
                }
            }
        }

        // Update is called once per frame			
        void Update()
        {
            if (m_playing)
            {
                if (m_paused == false)
                {
                    if (m_rhythm == null)
                    {
                        Debug.LogError("RhythmosEngine: RhythmosPlayer: No Rhythm is associated with RhythmosPlayer.\nThe rhythm can't not be played.");
                        m_playing = false;
                    }
                    else if (rhythmDatabase == null)
                    {
                        Debug.LogError("RhythmosEngine: RhythmosPlayer: No RhythmosDatabase is associated with this RhythmosPlayer.\nImpossible to play the rhythm.");
                        m_playing = false;
                    }

                    m_noteTime -= Time.deltaTime;
                    m_playedAmount += Time.deltaTime;
                    if (m_noteTime <= 0f)
                    {

                        m_noteIndex++;
                        if (m_rhythm != null)
                        {
                            if (m_noteIndex >= m_rhythm.Count)
                            {
                                m_noteIndex = -1;
                                m_noteTime = 0;
                                m_playedAmount = 0;
                                m_noteWasPlayed = false;

                                if (m_loop == false)
                                {
                                    m_playing = false;
                                }
                                else
                                {
                                    Play();
                                }

                                if (m_destroyOnEnd == true)
                                {
                                    GameObject.Destroy(gameObject);
                                    m_audioSourcesList.Clear();
                                }
                            }
                            else
                            {

                                m_noteTime = (60f / m_rhythm.BPM) * m_rhythm.Notes[m_noteIndex].duration;
                                m_noteWasPlayed = true;
                                if (!m_rhythm.Notes[m_noteIndex].isRest)
                                {
                                    if (rhythmDatabase != null)
                                    {
                                        AudioClip clip = rhythmDatabase.AudioReferences[m_rhythm.Notes[m_noteIndex].layoutIndex].Clip;
                                        if (clip != null)
                                        {
                                            AudioSource aS = PlaySound(clip, m_volume, false);
                                            m_audioSourcesList.Add(aS);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            m_playing = false;
                        }
                    }
                    else
                    {
                        m_noteWasPlayed = false;
                    }
                }
            }
            else
            {
                m_noteIndex = -1;
                m_noteTime = 0;
                m_noteWasPlayed = false;
            }
        }

        /// <summary>
        /// Determines if a note has played.
        /// </summary>
        /// <returns><c>true</c> if this instance has note played; otherwise, <c>false</c>.</returns>
        public bool HasNotePlayed()
        {
            return m_noteWasPlayed;
        }

        /// <summary>
        /// Gets the actual played note.
        /// </summary>
        /// <returns>The actual note.</returns>
        public Note GetCurrentNote()
        {
            Note note = new Note(-1, false, -1);

            if (m_rhythm != null && m_noteIndex >= 0)
            {
                note = m_rhythm.Notes[m_noteIndex];
            }

            return note;
        }

        /// <summary>
        /// Gets the last audio source.
        /// </summary>
        /// <returns>The last audio source.</returns>
        public AudioSource GetLastAudioSource()
        {
            return m_lastAudioSource;
        }

        /// <summary>
        /// Play RhythmosPlayer.
        /// </summary>
        public void Play()
        {
            m_playing = true;
            m_paused = false;
        }

        /// <summary>
        /// Stop the rhythm.
        /// </summary>
        public void Stop()
        {
            m_noteIndex = -1;
            m_noteTime = 0;
            m_playedAmount = 0;
            m_noteWasPlayed = false;
            m_playing = false;
            m_paused = false;

            foreach (AudioSource src in m_audioSourcesList)
            {
                src.Stop();
                GameObject.Destroy(src);
            }

            m_audioSourcesList.Clear();
        }

        /// <summary>
        /// Pause the rhythm.
        /// </summary>
        public void Pause()
        {
            m_paused = true;
            foreach (AudioSource src in m_audioSourcesList)
            {
                src.Pause();
            }
        }

        /// <summary>
        /// Unpause the rhythm
        /// </summary>
        public void UnPause()
        {
            m_paused = false;
            foreach (AudioSource src in m_audioSourcesList)
            {
                src.Play();
            }
        }

        private AudioSource PlaySound(AudioClip clip, float volume = 1f, bool loop = false)
        {

            GameObject obj = new GameObject("SoundEffect " + clip.name);
            obj.transform.parent = transform;

            AudioSource aSource = obj.AddComponent<AudioSource>();
            aSource.clip = clip;
            aSource.loop = loop;
            aSource.volume = volume;
            aSource.enabled = true;
            aSource.Play();
            m_lastAudioSource = aSource;
            return m_lastAudioSource;
        }

        /// <summary>
        /// Play a rhythm.
        /// </summary>
        /// <returns>The rhythm.</returns>
        /// <param name="rhythm">Rhythm.</param>
        /// <param name="database">Database.</param>
        /// <param name="volume">Volume.</param>
        /// <param name="loop">If set to <c>true</c> loop.</param>
        /// <param name="destroyOnEnd">If set to <c>true</c> destroy on end.</param>
        public static RhythmosPlayer PlayRhythm(Rhythm rhythm, RhythmosDatabase database, float volume = 1f, bool loop = false, bool destroyOnEnd = true)
        {

            RhythmosPlayer player = null;
            if (rhythm != null)
            {
                if (database != null)
                {
                    GameObject obj = new GameObject("RhythmosPlayer - " + rhythm.Name);
                    player = obj.AddComponent<RhythmosPlayer>();
                    player.m_rhythm = rhythm;
                    player.m_rhythmDatabase = database;
                    player.m_volume = volume;
                    player.m_loop = loop;
                    player.m_destroyOnEnd = destroyOnEnd;
                    player.m_rhythmDuration = rhythm.Duration();
                    player.Play();

                }
                else
                {
                    Debug.LogError("RhythmosEngine: RhythmosPlayer: Reference of RhythmDatabase is null. \nImpossible to play Rhythm.");
                }
            }
            else
            {
                Debug.LogError("RhythmosEngine: RhythmosPlayer: Reference of Rhythm is null.\nRhythm was not played.");
            }
            return player;
        }
    }
}

