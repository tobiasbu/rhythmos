using UnityEngine;
using UnityEditor;
using RhythmosEngine;

namespace RhythmosEditor.Pages
{
    internal class SettingsPage : IEditorPage
    {
        Config config;

        public IUndoRedoDelegate UndoRedoDelegate => null;

        public void OnDraw(Rect pageRect)
        {
            GUILayout.Label("Database Settings:", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Rhythmos Database:", config.lastOpenDatabase, GUILayout.Width(pageRect.width * 0.7f));

            if (GUILayout.Button("Import"))
            {
                string sourcePath = EditorUtility.OpenFilePanel("Open Rhythmos Database", Application.dataPath, "xml");
                RhythmosDatabase database = config.LoadDatabaseXML(sourcePath);
            }

            if (GUILayout.Button("New"))
            {
                string path = EditorUtility.SaveFilePanelInProject("New Rhythmos Database", "RhythmosDatabase", "xml", "Please enter a file name to create new database.");
                if (!string.IsNullOrEmpty(path))
                {
                    Debug.Log("Created new Rhythmos Database in " + path);
                    config.SaveDatabaseXML(path);
                    config.lastOpenDatabase = path;
                }

            }

            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox(config.statusMessage, config.statusType);
            GUILayout.BeginHorizontal(GUILayout.Width(pageRect.width * 0.5f));

            if (!config.loaded)
            {
                GUI.enabled = false;
            }

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


            GUILayout.EndHorizontal();

            if (!config.loaded)
            {
                GUI.enabled = false;
            }

            GUILayout.Label("Editor Settings:", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Metronome Audio Clip:", EditorStyles.label);
            EditorGUI.BeginChangeCheck();
            AudioClip metro = (AudioClip)EditorGUILayout.ObjectField(config.metroAudioClip, typeof(AudioClip), false);
            if (EditorGUI.EndChangeCheck())
            {
                config.metroAudioClip = metro;
            }
            GUILayout.EndHorizontal();
        }

        public void OnLoad()
        {
           
        }

        public void OnPageSelect(Config config)
        {
            this.config = config;
        }

        public void OnUpdate(Event guiEvent)
        {
            
        }
    }
}
