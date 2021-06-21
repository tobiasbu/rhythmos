using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using RhythmosEngine;

namespace RhythmosEditor
{
    internal class RhythmosEditorNoteListTab
    {
        public List<AudioReference> LayoutList { set; get; }

        [SerializeField]
        private UndoAudioReferenceManager _undoManager;

        private int _selected = -1;
        private int _lastselected = -1;
        float vSbarValue = 0;

        public RhythmosEditorNoteListTab()
        {

            if (_undoManager == null)
                _undoManager = new UndoAudioReferenceManager();
            else
                _undoManager.Clear();
        }

        //Public functions
        public void AddEntry(AudioReference entry)
        {
            LayoutList.Add(entry);
        }

        public void RemoveEntry(AudioReference entryToRemove)
        {
            LayoutList.Remove(entryToRemove);
        }

        public void Clear()
        {
            if (LayoutList != null)
            {
                LayoutList.Clear();
            }
        }

        public void ReinsertEntry(AudioReference entry, int index)
        {
            LayoutList.RemoveAt(index);
            LayoutList.Insert(index, entry);
        }

        public void DeleteAt(int index)
        {
            if (LayoutList.Count != 0)
            {
                LayoutList.RemoveAt(index);
            }
            else
            {
                _selected = -1;
            }
        }

        public int GetSelectedItem()
        {
            return _selected;
        }

        public bool SelectionChanged()
        {
            if (_lastselected != _selected)
                return true;
            else
                return false;
        }

        public void ClearUndoRedo()
        {
            _undoManager.Clear();
        }

        void PerformUndo()
        {

            UndoAudioReference undo = _undoManager.Undo();

            if (undo.undoAction != "none")
            {

                if (undo.undoAction == "New Note Layout")
                {

                    if (LayoutList.Count != 0)
                    {
                        LayoutList.RemoveAt(undo.index);
                    }

                }
                else if (undo.undoAction == "Remove Note Layout")
                {

                    LayoutList.Insert(undo.index, undo.note);

                }
                else
                {


                    LayoutList[undo.index] = undo.note;

                }

                _undoManager.RecordNoteRedo(undo.note, undo.undoAction, undo.index);
                _undoManager.RemoveLastUndo();

            }

        }

        void PerformRedo()
        {

            UndoAudioReference redo = _undoManager.Redo();

            if (redo.undoAction != "none")
            {

                if (redo.undoAction == "New Note Layout")
                {

                    LayoutList.Insert(redo.index, redo.note);


                }
                else if (redo.undoAction == "Remove Note Layout")
                {

                    if (LayoutList.Count != 0)
                    {
                        LayoutList.RemoveAt(redo.index);
                    }

                }
                else
                {
                    LayoutList[redo.index] = redo.note;
                }
                _undoManager.RecordNote(redo.note, redo.undoAction, redo.index, false);
                _undoManager.RemoveLastRedo();
            }
        }

        public void DrawHeader(Rect Area, Texture2D undo, Texture2D redo)
        {

            //GUILayout.BeginArea(new Rect(Area.x, Area.y, Area.width, Area.height), "");
            //GUILayout.BeginHorizontal();

            //if (_undoManager.UndoCount == 0)
            //    GUI.enabled = false;
            //else
            //    GUI.enabled = true;

            //Color oldColor = GUI.contentColor;
            //if (!EditorGUIUtility.isProSkin)
            //{
            //    GUI.contentColor = EditorStyles.label.normal.textColor;
            //}

            //if (GUILayout.Button(new GUIContent(undo, "Undo"), GUILayout.Width(24), GUILayout.Height(19)))
            //{
            //    PerformUndo();
            //}

            //if (_undoManager.RedoCount == 0)
            //    GUI.enabled = false;
            //else
            //    GUI.enabled = true;

            //if (GUILayout.Button(new GUIContent(redo, "Redo"), GUILayout.Width(24), GUILayout.Height(19)))
            //{
            //    PerformRedo();
            //}

            //GUI.contentColor = oldColor;

            //GUI.enabled = true;

            if (GUILayout.Button("New Note Layout", GUILayout.Width(120)))
            {
                LayoutList.Add(new AudioReference());
                //             Color = new Color(UnityEngine.Random.Range(0.3f, 0.95f), UnityEngine.Random.Range(0.3f, 0.95f), UnityEngine.Random.Range(0.3f, 0.95f));
                _undoManager.RecordNote(LayoutList[LayoutList.Count - 1], "New Note Layout", LayoutList.Count - 1, true);
                vSbarValue = LayoutList.Count;
            }

            //GUILayout.EndHorizontal();
            //GUILayout.EndArea();
        }

        public void Draw(Rect Area, float ItemHeight, Color BackgroundColor, Color SelectedItemColor)
        {

            AudioReference _s;
            Rect listBox = new Rect(10, Area.y + 16 + 1, Area.width - 5, Area.height - 32 - 2);

            float totalsize = (ItemHeight * LayoutList.Count);
            float entryWidth;
            float _y = -(ItemHeight * vSbarValue) + 5;

            if (totalsize >= Area.height - 32)
                entryWidth = Area.width - 30;
            else
                entryWidth = Area.width - 15;

            GUI.color = BackgroundColor;


            GUILayout.BeginArea(new Rect(5, Area.y, entryWidth, 16), "");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("ID", EditorStyles.boldLabel, GUILayout.Width(entryWidth * 0.05f));
            GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(entryWidth * 0.35f));
            GUILayout.Label("Color", EditorStyles.boldLabel, GUILayout.Width(entryWidth * 0.1f));
            GUILayout.Label("Audio Clip", EditorStyles.boldLabel, GUILayout.Width(entryWidth * 0.3f));
            GUILayout.Label("Remove", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();

            GUI.Box(new Rect(5, Area.y + 16, Area.width, Area.height - 32), "");

            //Draw the listbox.
            GUI.BeginGroup(listBox, "");

            GUI.color = Color.black;

            GUI.color = Color.white;

            //Loop through to draw the entries and check for selection.
            for (int i = 0; i < LayoutList.Count; i++)
            {
                //Get the list entry's name
                _s = LayoutList[i];

                //Get the selection's area.
                Rect _entryBox;

                _entryBox = new Rect(0, _y, entryWidth, ItemHeight);

                GUILayout.BeginArea(_entryBox, "");
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString() + ".", GUILayout.Width(_entryBox.width * 0.05f));
                //EditorGUI.BeginChangeCheck();
                //string name = EditorGUILayout.TextField(_s.Name, GUILayout.Width(_entryBox.width * 0.35f));
                //if (EditorGUI.EndChangeCheck())
                //{
                //    _undoManager.RecordNote(_s, "Set Name", i, true);
                //    _s.Name = name;
                //    ReinsertEntry(_s, i);

                //}
                EditorGUI.BeginChangeCheck();
                Color color = EditorGUILayout.ColorField(_s.Color, GUILayout.Width(_entryBox.width * 0.1f));
                if (EditorGUI.EndChangeCheck())
                {
                    _undoManager.RecordNote(_s, "Set Color", i, true);
                    _s.Color = color;
                    ReinsertEntry(_s, i);
                }
                EditorGUI.BeginChangeCheck();
                AudioClip clip = (AudioClip)EditorGUILayout.ObjectField(_s.Clip, typeof(AudioClip), false, GUILayout.Width(_entryBox.width * 0.3f));
                if (EditorGUI.EndChangeCheck())
                {
                    _undoManager.RecordNote(_s, "Set Clip", i, true);
                    _s.Clip = clip;
                    ReinsertEntry(_s, i);
                }

                if (GUILayout.Button("Remove"))
                {
                    _undoManager.RecordNote(_s, "Remove Note Layout", i, true);
                    DeleteAt(i);
                }
                GUILayout.EndHorizontal();
                GUILayout.EndArea();
                _y += ItemHeight;
            }

            if (totalsize >= Area.height - 32)
            {
                float viewableRatio = ((Area.height - 32 - 2) / totalsize);
                vSbarValue = GUI.VerticalScrollbar(new Rect(Area.width - 21, 0, 30, Area.height - 32 - 2), vSbarValue, LayoutList.Count * viewableRatio, 0, LayoutList.Count);
            }
            else
            {
                vSbarValue = 0;
            }

            if (Event.current != null)
            {
                if (Event.current.type == EventType.ScrollWheel)
                {

                    if (listBox.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.delta.y > 0)
                        {

                            vSbarValue += 1;
                            if (vSbarValue >= LayoutList.Count)
                                vSbarValue = LayoutList.Count;

                        }
                        else if (Event.current.delta.y < 0)
                        {

                            vSbarValue -= 1;
                            if (vSbarValue <= 0)
                                vSbarValue = 0;

                        }
                    }

                }
            }

            GUI.EndGroup();
            GUI.color = Color.white;
        }
    }
}

