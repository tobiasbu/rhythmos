//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
        private List<Rhythm> m_rhythmDatabase;

        [SerializeField]
        private List<NoteLayout> m_noteLayoutList;

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.RhythmosDatabase"/>.
        /// </summary>
        public RhythmosDatabase()
        {
            m_rhythmDatabase = new List<Rhythm>();
            m_noteLayoutList = new List<NoteLayout>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RhythmosEngine.RhythmosDatabase"/> class.
        /// </summary>
        /// <param name="textAsset">Text asset.</param>
        [Obsolete("This constructor is deprecated and is unsafe. Please use 'Load' method instead.", true)]
        public RhythmosDatabase(TextAsset textAsset)
        {
            m_rhythmDatabase = new List<Rhythm>();
            m_noteLayoutList = new List<NoteLayout>();

            LoadRhythmosDatabase(textAsset);
        }

        /// <summary>
        /// Gets the rhythms count.
        /// </summary>
        /// <value>The rhythms count.</value>
        public int RhythmsCount
        {
            get
            {
                return m_rhythmDatabase.Count;
            }
        }

        /// <summary>
        /// Gets the note layout count.
        /// </summary>
        /// <value>The note layout count.</value>
        public int NoteLayoutCount
        {
            get
            {
                return m_noteLayoutList.Count;
            }
        }

        /// <summary>
        /// Gets the rhythm list.
        /// </summary>
        /// <returns>The rhythm list.</returns>
        public List<Rhythm> GetRhythmList()
        {
            return m_rhythmDatabase;
        }

        /// <summary>
        /// Gets the note layout list.
        /// </summary>
        /// <returns>The note layout list.</returns>
        public List<NoteLayout> GetNoteLayoutList()
        {
            return m_noteLayoutList;
        }

        /// <summary>
        /// Loads the rhythmos database.
        /// </summary>
        /// <returns><c>true</c>, if rhythmos database was loaded, <c>false</c> otherwise.</returns>
        /// <param name="resourcePath">Resource path.</param>
        [Obsolete("'LoadRhythmosDatabase' is deprecated and will removed in next version. Please use 'Load' method instead.", true)]
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
        [Obsolete("'LoadRhythmosDatabase' is deprecated and will removed in next version. Please use 'Load' method instead.", true)]
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
                    m_noteLayoutList.Clear();
                    m_rhythmDatabase.Clear();

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
                            NoteLayout nt = new NoteLayout();
                            foreach (KeyValuePair<string, string> entry in subDictionary)
                            {

                                if (entry.Key == "name")
                                    nt.Name = entry.Value;
                                else if (entry.Key == "color")
                                    nt.Color = Utility.ParseColor(entry.Value);
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
                            m_noteLayoutList.Add(nt);
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
                                            nt.duration = float.Parse(entry.Value);
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
                                        rtm.BPM = float.Parse(entry.Value);
                                }

                                AddRhythm(rtm);
                            }
                        }
                    }
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
        /// <exception cref="DatabaseLoadException">Throws if could not load RhythmosDatabase file correctly</exception
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
        /// In the future we'll provide a better way to load the RhtythmosDatabase using <see cref="ScriptableObject" />.
        /// </para>
        /// 
        /// </summary>
        /// <param name="textAsset">Text asset</param>
        /// <param name="loader">Implementation to load assets</param>
        /// <returns>Loaded RhythmosDatabse</returns>
        /// <exception cref="DatabaseLoadException">Throws if could not load RhythmosDatabase file correctly</exception>
        public static RhythmosDatabase Load(TextAsset textAsset, IAssetLoader loader)
        {
            RhythmosDatabase database = null;

            if (textAsset != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                MemoryStream assetStream = new MemoryStream(textAsset.bytes);
                XmlReader reader = XmlReader.Create(assetStream);

                try
                {
                    xmlDoc.Load(reader);
                    database = new RhythmosDatabase();

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
                            NoteLayout nt = new NoteLayout();
                            foreach (KeyValuePair<string, string> entry in subDictionary)
                            {

                                if (entry.Key == "name")
                                    nt.Name = entry.Value;
                                else if (entry.Key == "color")
                                    nt.Color = Utility.ParseColor(entry.Value);
                                else if (entry.Key == "audioclip")
                                {
                                    if (entry.Value == "0")
                                        nt.Clip = null;
                                    else
                                    {
                                        string path = entry.Value;
                                        nt.Clip = loader.LoadClip(path);
                                    }
                                }
                            }
                            database.AddNoteLayout(nt);
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
                                            nt.duration = float.Parse(entry.Value);
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
                                        rtm.BPM = float.Parse(entry.Value);
                                }

                                database.AddRhythm(rtm);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new DatabaseLoadException(" Error loading the TextAsset: " + textAsset.name, ex);   
                }
            }
            else
            {
                throw new DatabaseLoadException("TextAsset is null. Impossible to load RhythmosDatabase file."); 
            }

            return database;
        }

        /// <summary>
        /// Adds a rhythm
        /// </summary>
        /// <param name="newRhythm">New rhythm</param>
        public void AddRhythm(Rhythm newRhythm)
        {
            m_rhythmDatabase.Add(newRhythm);
        }

        /// <summary>
        /// Adds a note layout
        /// </summary>
        /// <param name="newNoteLayout">New note layout</param>
        public void AddNoteLayout(NoteLayout newNoteLayout)
        {
            m_noteLayoutList.Add(newNoteLayout);
        }

        /// <summary>
        /// Removes a rhythm at index.
        /// </summary>
        /// <param name="index">Index</param>
        public void RemoveRhythm(int index)
        {
            m_rhythmDatabase.RemoveAt(index);
        }

        /// <summary>
        /// Removes a note layout at index
        /// </summary>
        /// <param name="index">Index.</param>
        public void RemoveNoteLayout(int index)
        {
            m_noteLayoutList.RemoveAt(index);
        }

        /// <summary>
        /// Clears the rhythm list
        /// </summary>
        public void ClearRhythmList()
        {
            m_rhythmDatabase.Clear();
        }

        /// <summary>
        /// Clears the note layout list
        /// </summary>
        public void ClearNoteLayoutList()
        {
            m_noteLayoutList.Clear();
        }

        /// <summary>
        /// Finds a rhythm by name. 
        /// </summary>
        /// <returns>Rhythm. If null, the requested rhythm doesn't exist.</returns>
        /// <param name="name">Name.</param>
        public Rhythm FindRhythmByName(string name)
        {
            if (m_rhythmDatabase != null)
            {
                Rhythm founded = null;
                for (int i = 0; i < m_rhythmDatabase.Count; i++)
                {
                    if (m_rhythmDatabase[i].Name == name)
                    {
                        founded = m_rhythmDatabase[i];
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
        /// <param name="name">Name.</param>
        public NoteLayout FindNoteLayoutByName(string name)
        {
            if (m_noteLayoutList != null)
            {
                NoteLayout founded = null;
                for (int i = 0; i < m_noteLayoutList.Count; i++)
                {
                    if (m_noteLayoutList[i].Name == name)
                    {
                        founded = m_noteLayoutList[i];
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
        /// <returns>The Rhythm Player. If null, the requested rhythm doesn't exist.</returns>
        /// <param name="rhythmName">Rhythm name.</param>
        /// <param name="volume">Volume.</param>
        /// <param name="destroyOnEnd">If set to <c>true</c> destroy on end.</param>
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

