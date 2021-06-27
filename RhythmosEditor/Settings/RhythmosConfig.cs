using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using RhythmosEngine;
using RhythmosEditor.Utils;

namespace RhythmosEditor.Settings
{
    [Serializable]
    internal class RhythmosConfig : ISerializationCallbackReceiver
    {
        [SerializeField]
        private RhythmosDatabase rhythmosDatabase = null;

        [SerializeField]
        private bool fileExists = false;

        public RhythmosDatabase RhythmosDatabase
        {
            get
            {
                return rhythmosDatabase;
            }
        }

        public string lastOpenDatabase { set; get; }
        public AudioClip metroAudioClip { set; get; }

        public string statusMessage { private set; get; }
        public MessageType statusType { private set; get; }
        public bool loaded {
            get {
                return rhythmosDatabase != null;
            }
        }

        public RhythmosConfig()
        {
            statusMessage = "Please select a RhythmosDatabase file.";
            statusType = MessageType.None;
        }

        public void Save()
        {
            if (lastOpenDatabase == null && metroAudioClip == null)
            {
                return;
            }

            if (!Directory.Exists("Assets/Editor/"))
            {
                Directory.CreateDirectory("Assets/Editor");
            }

            FileStream file = File.Create("Assets/Editor/RhythmosEditorConfig.txt");
            StreamWriter writer = new StreamWriter(file);
            writer.WriteLine("RhythmosEditor - Configuration\n");
            if (lastOpenDatabase != null)
            {
                writer.WriteLine("LastRhythmosDatabaseFile: " + lastOpenDatabase);
            }
            if (metroAudioClip != null)
            {
                writer.WriteLine("MetronomeSound: " + AssetDatabase.GetAssetPath(metroAudioClip));
            }

            writer.Close();
            file.Close();

            AssetDatabase.Refresh();

        }

        public void Load()
        {

            if (File.Exists("Assets/Editor/RhythmosEditorConfig.txt"))
            {
                FileStream fileStream = File.OpenRead("Assets/Editor/RhythmosEditorConfig.txt");
                StreamReader reader = new StreamReader(fileStream);
                Regex rg = new Regex("^(.+): (.+)?");
                string key, value;

                lastOpenDatabase = null;

                while (!reader.EndOfStream)
                {
                    string text = reader.ReadLine();
                    Match match = rg.Match(text);
                    if (match.Success && match.Length > 1)
                    {
                        key = match.Groups[1].Value;
                        value = match.Groups[2].Value;
                        if (!match.Groups[2].Success)
                        {
                            continue;
                        }

                        if (!StringUtils.IsValid(value))
                        {
                            continue;
                        }

                        switch (key)
                        {
                            default:
                                break;
                            case "LastRhythmosDatabaseFile":
                                lastOpenDatabase = value;
                                break;
                            case "MetronomeSound":
                                AudioClip asset = (AudioClip)AssetDatabase.LoadAssetAtPath(value, typeof(AudioClip));
                                metroAudioClip = asset;
                                break;
                        }
                    }

                }


                reader.Close();
                fileStream.Close();
            }
        }

        public RhythmosDatabase LoadDatabaseXML(string sourcePath)
        {
            if (string.IsNullOrEmpty(sourcePath))
            {
                sourcePath = lastOpenDatabase;
            }

            RhythmosDatabase database = null;
            try
            {
                database = XMLDatabaseLoader.Import(sourcePath);
                lastOpenDatabase = AssetDatabase.GetAssetPath(database.TextAsset);
                statusMessage = "Database loaded successfully.";
                statusType = MessageType.Info;
            }
            catch (Exception ex)
            {
                fileExists = false;
                statusMessage = ex.Message;
                statusType = MessageType.Error;
                Debug.LogError(ex);
            }

            fileExists = database != null;
            rhythmosDatabase = database;

            return database;
        }

        public void SaveDatabaseXML(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                filepath = lastOpenDatabase;
            }

            if (rhythmosDatabase != null && StringUtils.IsValid(lastOpenDatabase) && statusType != MessageType.Error)
            {
                XMLDatabaseLoader.Export(rhythmosDatabase, filepath);
            }
        }

        public void OnBeforeSerialize()
        {
            if (!fileExists)
            {
                rhythmosDatabase = null;
            }
        }

        public void OnAfterDeserialize()
        {
            if (!fileExists)
            {
                rhythmosDatabase = null;
            }
        }
    }
}
