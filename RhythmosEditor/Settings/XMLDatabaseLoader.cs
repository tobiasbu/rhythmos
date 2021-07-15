using System;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEditor;
using RhythmosEngine;
using RhythmosEditor.Exceptions;
using RhythmosEditor.Utils;

namespace RhythmosEditor.Settings
{
    internal class XMLDatabaseLoader
    {
        /// <summary>
        /// Import RhythmosDatabase to Unity project
        /// </summary>
        /// <param name="sourcePath">File path to source database file</param>
        /// <exception cref="ImportException">Throws when importation fails</exception>
        /// <returns>Imported database</returns>
        public static RhythmosDatabase Import(string sourceFilePath)
        {
            if (!StringUtils.IsValid(sourceFilePath))
            {
                throw new ImportXmlException("Could not load RhythmosDatabase: sourcePath is null or empty.");
            }

            if (!File.Exists(sourceFilePath))
            {
                throw new ImportXmlException("Could not load RhythmosDatabase: " + sourceFilePath + " does no exist");
            }


            string assetPath = sourceFilePath;
            string directoryName = Path.GetDirectoryName(sourceFilePath);
            if (PathUtils.IsAssetsAbsolutePath(directoryName) || directoryName == "Assets")
            {
                int index = sourceFilePath.IndexOf("Assets");
                assetPath = sourceFilePath.Remove(0, index);
            }

            TextAsset textAsset = (TextAsset)AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset));
            return RhythmosDatabase.Load(textAsset, new EditorAssetLoader());

        }

        /// <summary>
        /// Export RhythmosDatabase to given file path.
        /// </summary>
        /// <param name="database">Database to export</param>
        /// <param name="filePath">File path destination</param>
        /// <exception cref="ExportException">Throws when exportation fails</exception>
        public static void Export(RhythmosDatabase database, string destinationFilePath)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement elmParent = xmlDoc.CreateElement("RhythmosDatabase");
                elmParent.SetAttribute("version", "1.3");

                XmlElement elmNoteList = xmlDoc.CreateElement("NoteList");
                foreach (AudioReference nt in database.AudioReferences)
                {

                    string audioStr = "0";
                    if (nt.Clip != null)
                    {
                        audioStr = AssetDatabase.GetAssetPath(nt.Clip);
                    }

                    XmlElement elmNoteEntry = xmlDoc.CreateElement("NoteEntry");
                    XmlNode node2 = XMLUtility.CreateNodeByName(xmlDoc, "color", StringUtils.Color(nt.Color));
                    XmlNode node3 = XMLUtility.CreateNodeByName(xmlDoc, "audioclip", audioStr);
                    elmNoteEntry.AppendChild(node2);
                    elmNoteEntry.AppendChild(node3);

                    elmNoteList.AppendChild(elmNoteEntry);

                }

                XmlElement elmRhythmList = xmlDoc.CreateElement("RhythmList");
                foreach (Rhythm rtm in database.Rhythms)
                {

                    XmlElement elmRhythmEntry = xmlDoc.CreateElement("RhythmEntry");
                    XmlElement elmRhythmSequence = xmlDoc.CreateElement("RhythmSequence");

                    XmlNode node0 = XMLUtility.CreateNodeByName(xmlDoc, "name", rtm.Name);
                    XmlNode node1 = XMLUtility.CreateNodeByName(xmlDoc, "bpm", rtm.BPM.ToString());

                    elmRhythmEntry.AppendChild(node0);
                    elmRhythmEntry.AppendChild(node1);

                    foreach (Note note in rtm.Notes)
                    {

                        XmlElement elmNote = xmlDoc.CreateElement("Note");
                        XmlNode nodeNote1 = XMLUtility.CreateNodeByName(xmlDoc, "rest", note.isRest.ToString());
                        XmlNode nodeNote2 = XMLUtility.CreateNodeByName(xmlDoc, "duration", StringUtils.Float(note.duration));
                        XmlNode nodeNote3 = XMLUtility.CreateNodeByName(xmlDoc, "layoutIndex", note.layoutIndex.ToString());

                        elmNote.AppendChild(nodeNote1);
                        elmNote.AppendChild(nodeNote2);
                        elmNote.AppendChild(nodeNote3);

                        elmRhythmSequence.AppendChild(elmNote);
                    }


                    elmRhythmEntry.AppendChild(elmRhythmSequence);

                    elmRhythmList.AppendChild(elmRhythmEntry);

                }

                elmParent.AppendChild(elmNoteList);
                elmParent.AppendChild(elmRhythmList);
                xmlDoc.AppendChild(declaration);
                xmlDoc.AppendChild(elmParent);

                xmlDoc.Save(destinationFilePath);

                string directoryName = Path.GetDirectoryName(destinationFilePath);
                if (PathUtils.IsAssetsAbsolutePath(directoryName) || directoryName == "Assets")
                {
                    AssetDatabase.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw new ExportXmlException("Could not export RhythmosDatabase to '" + destinationFilePath + "'", ex);
            }
        }
    }

}
