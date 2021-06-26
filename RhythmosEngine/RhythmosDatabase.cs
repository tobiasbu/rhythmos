using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace RhythmosEngine
{
    /// <summary>
    /// Provides a data storage for rhythms and note 
    /// layouts created from the RhythmosEditor to use in your game.
    /// </summary>
    [Serializable]
    public class RhythmosDatabase
    {
        [SerializeField]
        private TextAsset textAsset;

        [SerializeField]
        private List<Rhythm> rhythmList;

        [SerializeField]
        private List<AudioReference> audioReferences;

        /// <summary>
        /// Get TextAsset reference of this  <see cref="RhythmosEngine.RhythmosDatabase"/>.
        /// </summary>
        public TextAsset TextAsset
        {
            get
            {
                return textAsset;
            }
        }

        /// <summary>
        /// Gets the <see cref="RhythmosEngine.Rhythm"/> list contained in the <see cref="RhythmosEngine.RhythmosDatabase"/>.
        /// </summary>
        public List<Rhythm> Rhythms {
            get
            {
                return rhythmList;
            }
        }

        /// <summary>
        /// Gets the AudioClip reference list contained in the <see cref="RhythmosEngine.RhythmosDatabase"/>.
        /// </summary>
        public List<AudioReference> AudioReferences
        {
            get
            {
                return audioReferences;
            }
        }

        /// <summary>
        /// Gets the number of rhythms contained in the <see cref="RhythmosEngine.RhythmosDatabase"/>.
        /// </summary>
        public int RhythmsCount
        {
            get
            {
                return rhythmList != null ? rhythmList.Count : 0;
            }
        }


        /// <summary>
        /// Gets the number of note layout count contained in the <see cref="RhythmosEngine.RhythmosDatabase"/>.
        /// </summary>
        public int AudioReferencesCount
        {
            get
            {
                return audioReferences != null ? audioReferences.Count : 0;
            }
        }
        /// <summary>
        /// Get the version of the <see cref="RhythmosEngine.RhythmosDatabase"/>.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.RhythmosDatabase"/>.
        /// </summary>
        public RhythmosDatabase()
        {
            rhythmList = new List<Rhythm>();
            audioReferences = new List<AudioReference>();
            Version = "1.3";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.RhythmosDatabase"/>.
        /// </summary>
        /// <param name="textAsset">Text asset.</param>
        [Obsolete("This constructor is deprecated and is unsafe. Please use 'Load' static method instead.", true)]
        public RhythmosDatabase(TextAsset textAsset)
        {
            rhythmList = new List<Rhythm>();
            audioReferences = new List<AudioReference>();

            LoadRhythmosDatabase(textAsset);
        }

       

        /// <summary>
        /// Gets the rhythm list.
        /// </summary>
        /// <returns>The rhythm list.</returns>
        [Obsolete("'GetRhythmList' is deprecated and will be removed in the next version. Use Rhythms property instead.", true)]
        public List<Rhythm> GetRhythmList()
        {
            return rhythmList;
        }

        /// <summary>
        /// Gets the note layout list.
        /// </summary>
        /// <returns>The note layout list.</returns>
        [Obsolete("'GetNoteLayoutList' is deprecated and will be removed in the next version. Use AudioReferences property instead.", true)]
        public List<AudioReference> GetNoteLayoutList()
        {
            return audioReferences;
        }

        /// <summary>
        /// Loads the rhythmos database.
        /// </summary>
        /// <returns><c>true</c>, if rhythmos database was loaded, <c>false</c> otherwise.</returns>
        /// <param name="resourcePath">Resource path.</param>
        [Obsolete("'LoadRhythmosDatabase' is deprecated and will be removed in the next version. Please use 'Load' static method instead.", true)]
        public bool LoadRhythmosDatabase(string resourcePath)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(resourcePath, typeof(TextAsset));
            if (textAsset != null)
            {
                LoadRhythmosDatabase(textAsset);
                return true;
            }
            else
            {
                Debug.LogError("RhythmosEngine: The TextAsset is null. Impossible to load RhythmosDatabase file.");
                return false;
            }

        }

        /// <summary>
        /// Loads Rhythmos Database from a Text Asset (XML document).
        /// </summary>
        /// <returns><c>true</c>, if rhythmos database was loaded, <c>false</c> otherwise.</returns>
        /// <param name="textAsset">Text asset.</param>
        [Obsolete("'LoadRhythmosDatabase' is deprecated and will be removed in the next version. Please use 'Load' static method instead.", true)]
        public bool LoadRhythmosDatabase(TextAsset textAsset)
        {
            if (textAsset != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream assetStream = new MemoryStream(textAsset.bytes);
                XmlReader reader = XmlReader.Create(assetStream);

                try
                {
                    xmlDoc.Load(reader);
                    audioReferences.Clear();
                    rhythmList.Clear();

                    XmlNodeList noteList = xmlDoc.GetElementsByTagName("NoteEntry");
                    foreach (XmlNode node in noteList)
                    {
                        Dictionary<string, string> subDictionary = new Dictionary<string, string>();
                        XmlNodeList noteContet = node.ChildNodes;

                        foreach (XmlNode noteItens in noteContet)
                        {
                            if (noteItens.Name == "name" || noteItens.Name == "audioclip" || noteItens.Name == "color")
                            {
                                subDictionary.Add(noteItens.Name, noteItens.InnerText);
                            }
                        }

                        if (subDictionary.Count == 3)
                        {
                            AudioReference nt = new AudioReference();
                            foreach (KeyValuePair<string, string> entry in subDictionary)
                            {

                                if (entry.Key == "color")
                                    nt.Color = Parse.Color(entry.Value);
                                else if (entry.Key == "audioclip")
                                {
                                    if (entry.Value == "0")
                                        nt.Clip = null;
                                    else
                                    {
                                        string field = entry.Value;
                                        field = field.Replace("Assets/Resources/", "");
                                        field = field.Remove(field.Length - 4, 4);
                                        AudioClip a = (AudioClip)Resources.Load(field, typeof(AudioClip));

                                        if (a != null)
                                        {
                                            nt.Clip = a;
                                        }
                                        else
                                        {
                                            nt.Clip = null;
                                        }
                                    }
                                }
                            }
                            audioReferences.Add(nt);
                        }
                    }

                    XmlNodeList RhythmEntry = xmlDoc.GetElementsByTagName("RhythmEntry");
                    foreach (XmlNode node in RhythmEntry)
                    {

                        Dictionary<string, string> subDictionary = new Dictionary<string, string>();
                        XmlNodeList rhythmContent = node.ChildNodes;
                        Rhythm rtm = new Rhythm();

                        foreach (XmlNode rhythmItens in rhythmContent)
                        {
                            if (rhythmItens.Name == "name" || rhythmItens.Name == "bpm")
                            {
                                subDictionary.Add(rhythmItens.Name, rhythmItens.InnerText);
                            }
                            else if (rhythmItens.Name == "RhythmSequence")
                            {

                                XmlNodeList sequence = rhythmItens.SelectNodes("./Note");
                                foreach (XmlNode nodeNote in sequence)
                                {
                                    XmlNodeList childSequence = nodeNote.ChildNodes;
                                    Dictionary<string, string> dictionarySequence = new Dictionary<string, string>();

                                    foreach (XmlNode nodeNoteItens in childSequence)
                                    {
                                        if (nodeNoteItens.Name == "rest" || nodeNoteItens.Name == "duration" || nodeNoteItens.Name == "layoutIndex")
                                        {
                                            dictionarySequence.Add(nodeNoteItens.Name, nodeNoteItens.InnerText);
                                        }
                                    }

                                    Note nt = new Note();
                                    foreach (KeyValuePair<string, string> entry in dictionarySequence)
                                    {
                                        if (entry.Key == "duration")
                                            nt.duration = Parse.Float(entry.Value);
                                        else if (entry.Key == "rest")
                                            nt.isRest = bool.Parse(entry.Value);
                                        else if (entry.Key == "layoutIndex")
                                            nt.layoutIndex = int.Parse(entry.Value);
                                    }

                                    rtm.AppendNote(nt);
                                }

                                foreach (KeyValuePair<string, string> entry in subDictionary)
                                {
                                    if (entry.Key == "name")
                                        rtm.Name = entry.Value;
                                    else if (entry.Key == "bpm")
                                        rtm.BPM = Parse.Float(entry.Value);
                                }

                                AddRhythm(rtm);
                            }
                        }
                    }
                    this.textAsset = textAsset;
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.LogError("RhythmosEngine: RhythmosDatabase: Error loading the TextAsset: " + textAsset.name + ".\n" + ex.ToString());
                    return false;
                }
            }
            else
            {
                Debug.LogError("RhythmosEngine: RhythmosDatabase: The TextAsset is null. Impossible to load RhythmosDatabase file.");
                return false;
            }
        }

        /// <summary>
        /// Loads RhythmosDatabas by given resource path ("Assets/Resources/...").
        /// </summary>
        /// <param name="resourcePath">Resource path</param>
        /// <returns>Loaded RhythmosDatabse</returns>
        /// <exception cref="DatabaseLoadException">Throws if could not load RhythmosDatabase file correctly</exception>
        public static RhythmosDatabase Load(string resourcePath)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(resourcePath, typeof(TextAsset));
            return Load(textAsset);
        }

        /// <summary>
        /// Load RhythmosDatabase from a TextAsset (XML file).
        /// </summary>
        /// <param name="textAsset">Text asset</param>
        /// <returns>Loaded RhythmosDatabse</returns>
        /// <exception cref="DatabaseLoadException">Throws if could not load RhythmosDatabase file correctly</exception>
        public static RhythmosDatabase Load(TextAsset textAsset)
        {
            return Load(textAsset, new ResourceAssetLoader());
        }
        
        /// <summary>
        /// Load RhythmosDatabase from a TextAsset (XML file).
        /// 
        /// <para>
        /// NOTE: The <paramref name="loader"/> parameter defines how the RhythmDatabase will load AudioClips.
        /// In Unity to load an asset in runtime you need to use <see cref="Resources"/> utility. 
        /// To work properly with RhythmosDatabase, make sure to put your assets inside "Resources" folder.
        /// </para>
        /// 
        /// <para>
        /// In the future we'll provide a better way to manage the RhtythmosDatabase using <see cref="ScriptableObject" />.
        /// </para>
        /// 
        /// </summary>
        /// <param name="textAsset">Text asset</param>
        /// <param name="loader">Implementation to load assets</param>
        /// <returns>Loaded RhythmosDatabse</returns>
        /// <exception cref="DatabaseLoadException">Throws if could not load RhythmosDatabase file correctly</exception>
        public static RhythmosDatabase Load(TextAsset textAsset, IAssetLoader loader)
        {
            RhythmosDatabase database;
            if (textAsset != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream assetStream = new MemoryStream(textAsset.bytes);
                XmlReader reader = XmlReader.Create(assetStream);
                try
                {
                    xmlDoc.Load(reader);
                    database = new RhythmosDatabase();
                    database.textAsset = textAsset;

                    // Parse version
                    XmlNodeList root = xmlDoc.GetElementsByTagName("RhythmosDatabase");
                    if (root.Count > 0)
                    {
                        XmlAttribute xmlAttribute = root[0].Attributes["version"];
                        if (xmlAttribute != null)
                        {
                            database.Version = Parse.IsValid(xmlAttribute.Value) ? xmlAttribute.Value.Trim() : "1.1";
                        }
                    }

                    XmlNodeList noteList = xmlDoc.GetElementsByTagName("NoteEntry");
                    foreach (XmlNode node in noteList)
                    {
                        Dictionary<string, string> subDictionary = new Dictionary<string, string>();
                        XmlNodeList noteContet = node.ChildNodes;

                        foreach (XmlNode noteItens in noteContet)
                        {
                            if (noteItens.Name == "name" || noteItens.Name == "audioclip" || noteItens.Name == "color")
                            {
                                subDictionary.Add(noteItens.Name, noteItens.InnerText);
                            }
                        }
                        
                        if (subDictionary.Count > 0)
                        {
                            Color color = Color.black;
                            AudioClip clip = null;
                            foreach (KeyValuePair<string, string> entry in subDictionary)
                            {
                                if (entry.Key == "color")
                                {
                                    color = Parse.Color(entry.Value);
                                }
                                else if (entry.Key == "audioclip")
                                {
                                    if (entry.Value != "0")
                                    {
                                        clip = loader.LoadClip(entry.Value);
                                    }
                                }
                            }
                            database.AddAudioClipReference(new AudioReference(clip, color));
                        }
                    }

                    XmlNodeList RhythmEntry = xmlDoc.GetElementsByTagName("RhythmEntry");
                    foreach (XmlNode node in RhythmEntry)
                    {
                        Dictionary<string, string> subDictionary = new Dictionary<string, string>();
                        XmlNodeList rhythmContent = node.ChildNodes;
                        Rhythm rtm = new Rhythm();

                        foreach (XmlNode rhythmItens in rhythmContent)
                        {
                            if (rhythmItens.Name == "name" || rhythmItens.Name == "bpm")
                            {
                                subDictionary.Add(rhythmItens.Name, rhythmItens.InnerText);
                            }
                            else if (rhythmItens.Name == "RhythmSequence")
                            {
                                XmlNodeList sequence = rhythmItens.SelectNodes("./Note");
                                foreach (XmlNode nodeNote in sequence)
                                {
                                    XmlNodeList childSequence = nodeNote.ChildNodes;
                                    Dictionary<string, string> dictionarySequence = new Dictionary<string, string>();

                                    foreach (XmlNode nodeNoteItens in childSequence)
                                    {
                                        if (nodeNoteItens.Name == "rest" || nodeNoteItens.Name == "duration" || nodeNoteItens.Name == "layoutIndex")
                                        {
                                            dictionarySequence.Add(nodeNoteItens.Name, nodeNoteItens.InnerText);
                                        }
                                    }

                                    Note nt = new Note();
                                    foreach (KeyValuePair<string, string> entry in dictionarySequence)
                                    {
                                        if (entry.Key == "duration")
                                            nt.duration = Parse.Float(entry.Value);
                                        else if (entry.Key == "rest")
                                            nt.isRest = bool.Parse(entry.Value);
                                        else if (entry.Key == "layoutIndex")
                                            nt.layoutIndex = int.Parse(entry.Value);
                                    }
                                    rtm.AppendNote(nt);
                                }

                                foreach (KeyValuePair<string, string> entry in subDictionary)
                                {
                                    if (entry.Key == "name")
                                        rtm.Name = entry.Value;
                                    else if (entry.Key == "bpm")
                                        rtm.BPM = Parse.Float(entry.Value);
                                }

                                database.AddRhythm(rtm);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseLoadException("Error loading the TextAsset: " + textAsset.name, ex);   
                }
            }
            else
            {
                throw new DatabaseLoadException("TextAsset is null. Impossible to load RhythmosDatabase file."); 
            }

            return database;
        }

        /// <summary>
        /// Adds a <see cref="Rhythm"/> to this database
        /// </summary>
        /// <param name="newRhythm">New rhythm</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="newRhythm"/> is null</exception>
        public void AddRhythm(Rhythm newRhythm)
        {
            if (newRhythm == null)
            {
                throw new ArgumentNullException("newRhythm", "RhythmosEngine: could not add Rhythm to RhythmosDatabase.");
            }

            rhythmList.Add(newRhythm);
        }

        /// <summary>
        /// Adds a note layout
        /// </summary>
        /// <param name="newNoteLayout">New note layout</param>
        [Obsolete("'AddNoteLayout' is deprecated and will be removed in the next version. Use 'AddAudioClipReference' instead", true)]
        public void AddNoteLayout(NoteLayout newNoteLayout)
        {
            //audioClipReferences.Add(newNoteLayout);
        }

        /// <summary>
        /// Adds an <see cref="AudioClip"/> reference to <see cref="RhythmosEngine.RhythmosDatabase"/>
        /// </summary>
        /// <param name="audioClipReference">AudioClip referencet</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="audioClipReference"/> is null</exception>
        public void AddAudioClipReference(AudioReference audioClipReference)
        {
            if (audioClipReference == null)
            {
                throw new ArgumentNullException("audioClipReference", "RhythmosEngine: could not add AudioReference to RhythmosDatabase.");
            }

            audioReferences.Add(audioClipReference);
        }

        /// <summary>
        /// Removes a rhythm at index.
        /// </summary>
        /// <param name="index">Index</param>
        public void RemoveRhythm(int index)
        {
            rhythmList.RemoveAt(index);
        }

        /// <summary>
        /// Removes a note layout at index
        /// </summary>
        /// <param name="index">Index.</param>
        [Obsolete("'RemoveNoteLayout' is deprecated and will be removed in the next version. Use 'RemoveAudioClipReference' instead", true)]
        public void RemoveNoteLayout(int index)
        {
            audioReferences.RemoveAt(index);
        }

        /// <summary>
        /// Removes an AudioClipReference at given index
        /// </summary>
        /// <param name="index">Index to be removed</param>
        public void RemoveAudioClipReference(int index)
        {
            audioReferences.RemoveAt(index);
        }

        /// <summary>
        /// Clears the rhythm list
        /// </summary>
        [Obsolete("'ClearRhythmList' is deprecated and will be removed in the next version. Use 'Rhythms.Clear()' property", false)]
        public void ClearRhythmList()
        {
            rhythmList.Clear();
        }

        /// <summary>
        /// Clears the note layout list
        /// </summary>
        [Obsolete("'ClearRhythmList' is deprecated and will be removed in the next version. Use 'AudioReferences.Clear()' property", false)]
        public void ClearNoteLayoutList()
        {
            audioReferences.Clear();
        }

        /// <summary>
        /// Finds a <see cref="Rhythm"/> by given name
        /// </summary>
        /// <returns>Rhythm instance otherwise null.</returns>
        /// <param name="name">Rhythm name</param>
        public Rhythm FindRhythmByName(string name)
        {
            if (rhythmList != null)
            {
                Rhythm founded = null;
                for (int i = 0; i < rhythmList.Count; i++)
                {
                    if (rhythmList[i].Name == name)
                    {
                        founded = rhythmList[i];
                        return founded;
                    }
                }
                return founded;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds note layout by name.
        /// </summary>
        /// <returns>Note layout. If null, the requested Note Layout doesn't exist.</returns>
        /// <param name="name">NoteLayout name</param>
        [Obsolete("'FindNoteLayoutByName' is deprecated and will be removed in the next version. Use 'FindAudioReferenceByName' property", false)]
        public AudioReference FindNoteLayoutByName(string name)
        {
            if (audioReferences != null)
            {
                AudioReference founded = null;
                for (int i = 0; i < audioReferences.Count; i++)
                {
                    if (audioReferences[i].Clip != null && audioReferences[i].Clip.name == name)
                    {
                        founded = audioReferences[i];
                        return founded;
                    }
                }
                return founded;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds AudioClipReference by given <see cref="AudioClip"/> name
        /// </summary>
        /// <returns>Returns AudioReference tuple, otherwise null</returns>
        /// <param name="name"><see cref="AudioClip"/> name</param>
        public AudioReference FindAudioReferenceByName(string name)
        {
            if (audioReferences != null)
            {
                AudioReference founded = null;
                for (int i = 0; i < audioReferences.Count; i++)
                {
                    if (audioReferences[i].Clip != null && audioReferences[i].Clip.name == name)
                    {
                        founded = audioReferences[i];
                        return founded;
                    }
                }
                return founded;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Play a rhythm.
        /// </summary>
        /// <returns>The Rhythm Player. If null, the requested rhythm doesn't exist</returns>
        /// <param name="rhythmName">Rhythm name</param>
        /// <param name="volume">Volume</param>
        /// <param name="loop">Should rhyhthm loop?</param>
        /// <param name="destroyOnEnd">If set to <c>true</c> destroy on end</param>
        public RhythmosPlayer PlayRhythm(string rhythmName, float volume = 1f, bool loop = false, bool destroyOnEnd = true)
        {
            Rhythm rhythm = FindRhythmByName(rhythmName);
            RhythmosPlayer rhythmoPlayer = null;
            if (rhythm != null)
            {
                rhythmoPlayer = RhythmosPlayer.PlayRhythm(rhythm, this, volume, loop, destroyOnEnd);
            }
            return rhythmoPlayer;
        }
    }
}

