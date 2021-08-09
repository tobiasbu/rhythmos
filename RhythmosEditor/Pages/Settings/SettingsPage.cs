using UnityEngine;
using UnityEditor;
using RhythmosEditor.Settings;
using RhythmosEditor.UIComponents;

namespace RhythmosEditor.Pages
{
    internal class SettingsPage : IEditorPage
    {
        RhythmosConfig config;

        public void OnDraw(Rect pageRect)
        {

            Components.Header("Database");
            GUILayout.Space(2);

            EditorGUILayout.HelpBox(config.statusMessage, config.statusType, true);

            GUILayout.Space(2);

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Loaded Asset", config.TextAsset, typeof(TextAsset), false);

            GUI.enabled = config.loaded;

            if (GUILayout.Button("Export"))
            {
                string path = EditorUtility.SaveFilePanel("Save As Rhythmos Database", Application.dataPath, "RhythmosDatabase", "xml");
                if (!string.IsNullOrEmpty(path))
                {
                    Debug.Log("Saved Rhythmos Database in " + path);
                    config.SaveDatabaseXML(path);
                }

            }

            GUI.enabled = true;

            if (GUILayout.Button("Import"))
            {
                string sourcePath = EditorUtility.OpenFilePanel("Open Rhythmos Database", Application.dataPath, "xml");
                config.LoadDatabaseXML(sourcePath);
            }


            if (GUILayout.Button("New"))
            {
                string path = EditorUtility.SaveFilePanelInProject("New Rhythmos Database", "RhythmosDatabase", "xml", "Please enter a file name to create new database.");
                if (!string.IsNullOrEmpty(path))
                {
                    Debug.Log("Created new Rhythmos Database in " + path);
                    config.CreateDatabaseXML(path);
                }

            }


            Components.Header("RhythmosEditor Settings");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Metronome Audio Clip:", EditorStyles.label);
            EditorGUI.BeginChangeCheck();
            AudioClip metro = (AudioClip)EditorGUILayout.ObjectField(config.metroAudioClip, typeof(AudioClip), false);
            if (EditorGUI.EndChangeCheck())
            {
                config.metroAudioClip = metro;
            }
            GUILayout.EndHorizontal();


            Components.Header("About");
            GUILayout.Label("RhythmosEngine v1.3");
            Components.LinkLabel("GitHub repository", "https://github.com/tobiasbu/rhythmos-engine");

            GUILayout.Label("Created by @tobiasbu");
            GUILayout.Label("2015-2021 - (C) MIT License");


            GUILayout.FlexibleSpace();
        }


        public void OnLoad()
        {

        }

        public void OnPageSelect(RhythmosConfig config)
        {
            this.config = config;
        }

    }
}
