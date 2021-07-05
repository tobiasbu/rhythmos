using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RhythmosEngine;
using RhythmosEditor.UIComponents;
using RhythmosEditor.UI;

namespace RhythmosEditor
{

    internal class RhythmosEditorRhythmListTab
    {
        private ListView<Rhythm> rhythmListView;

        private GUIContent addContentButton;
        private GUIContent copyContentButton;
        private GUIContent deleteContentButton;

        private EditorWindow mainWindow;
        private List<AudioReference> _noteList;

        private UndoRhythmManager _undoManager;

        private RhythmosEditorTimeline _timeline;

        private GUIStyle listItemStyle;

        bool _mouseChanged2 = false;
        float vSbarValueNl = 0;
        Vector2 _mpos2 = new Vector2(-99, -99);


        public List<Rhythm> RhythmList { set; get; }

        public RhythmosEditorRhythmListTab(EditorWindow mainWindow)
        {

            if (_timeline == null)
                _timeline = new RhythmosEditorTimeline();

            if (_undoManager == null)
                _undoManager = new UndoRhythmManager();
            else
                _undoManager.Clear();

            this.mainWindow = mainWindow;
        }
        public void Load()
        {
            if (listItemStyle == null)
            {
                listItemStyle = new GUIStyle("label");
                GUIStyleState normalStyle = EditorStyles.label.normal;
                listItemStyle.normal = normalStyle;
                listItemStyle.normal.textColor = Color.white;
                listItemStyle.contentOffset = new Vector2(2, -1);
            }

            if (rhythmListView == null)
            {
                rhythmListView = new ListView<Rhythm>();
                rhythmListView.onGetItemLabel = (item) => item.Name;
            }

            if (addContentButton == null)
            {
                addContentButton = new GUIContent(Icons.Add, "Create new rhythm");
            }

            if (copyContentButton == null)
            {
                copyContentButton = new GUIContent(Icons.Duplicate, "Duplicate selected rhythm");
            }

            if (deleteContentButton == null)
            {
                deleteContentButton = new GUIContent(Icons.Trash, "Remove selected rhythm");
            }

            
        }

        private void AddRhythm(string Name)
        {

            Rhythm newRhythm = new Rhythm(Name, 80);
            newRhythm.BPM = 80;
            RhythmList.Add(newRhythm);

        }

        private void DeleteRhythmAt(int index)
        {
            if (RhythmList.Count > 0)
            {
                RhythmList.RemoveAt(index);
            }
            else
            {
                rhythmListView.UnSelect();
            }
        }

        public void Clear()
        {
            if (RhythmList != null)
            {
                RhythmList.Clear();
            }
        }
        //public void RenameRhythm(string name, int index)
        //{
        //    Rhythm back = RhythmList[index];
        //    back.Name = name;
        //    RhythmList[index].Name = name;
        //}

        public void ClearRedoUndo()
        {
            _undoManager.Clear();
        }

        public void SetNoteLayoutList(List<AudioReference> list)
        {
            _timeline.SetNoteList(list);
            _noteList = list;
        }

        public void SetMetroSound(AudioClip sound)
        {
            _timeline.SetMetronomeAudioClip(sound);
        }

        public void Update()
        {
            _timeline.Playing();
        }

        void PerformRedo()
        {

            UndoRhythm redo = _undoManager.Redo();

            if (redo.undoAction != "none")
            {
                Rhythm rhythm;

                if (redo.undoAction == "Add New Rhythm" || redo.undoAction == "Duplicate Rhythm")
                {
                    _undoManager.RecordRhythm(redo.rhythm, redo.undoAction, redo.index, redo.lastSelectedNote, false);

                    if (RhythmList.Count == 0)
                        RhythmList.Add(new Rhythm(redo.rhythm));
                    else
                        RhythmList.Insert(redo.index, new Rhythm(redo.rhythm));

                    rhythm = RhythmList[redo.index];
                    _timeline.SetRhythm(rhythm);
                    _timeline.SetSelectedNoteIndex(redo.lastSelectedNote);

                    rhythmListView.Select(redo.index);
                }
                else if (redo.undoAction == "Remove Rhythm")
                {
                    if (RhythmList.Count != 0)
                    {
                        _undoManager.RecordRhythm(redo.rhythm, redo.undoAction, redo.index, redo.lastSelectedNote, false);
                        rhythmListView.Select(RhythmList.Count - 1);
                        _timeline.SetRhythm(rhythmListView.Current);
                        _timeline.SetSelectedNoteIndex(-1);
                    }
                }
                else
                {
                    _undoManager.RecordRhythm(redo.rhythm, redo.undoAction, redo.index, redo.lastSelectedNote, false);
                    RhythmList[redo.index] = redo.rhythm;
                    rhythm = RhythmList[redo.index];
                    _timeline.SetRhythm(rhythm);
                    _timeline.SetSelectedNoteIndex(redo.lastSelectedNote);
                    rhythmListView.Select(redo.index);

                }
                _undoManager.RemoveLastRedo();
            }
        }

        void PerformUndo()
        {
            UndoRhythm undo = _undoManager.Undo();
            if (undo.undoAction != "none")
            {
                Rhythm rhythm;

                if (undo.undoAction == "Add New Rhythm" || undo.undoAction == "Duplicate Rhythm")
                {
                    _undoManager.RecordRhythmRedo(undo.rhythm, undo.undoAction, undo.index, undo.lastSelectedNote);

                    if (RhythmList.Count != 0)
                    {
                        if (RhythmList.Count == 1)
                        {
                            RhythmList.Clear();
                            rhythmListView.UnSelect();
                        }
                        else
                        {
                            RhythmList.RemoveAt(undo.index);
                            rhythmListView.Select(RhythmList.Count - 1);
                        }

                    }

                    _timeline.SetRhythm(rhythmListView.Current);
                    _timeline.SetSelectedNoteIndex(-1);

                }
                else if (undo.undoAction == "Remove Rhythm")
                {

                    if (RhythmList.Count == 0)
                    {
                        RhythmList.Add(new Rhythm(undo.rhythm));
                        rhythm = RhythmList[0];
                    }
                    else
                    {
                        RhythmList.Insert(undo.index, new Rhythm(undo.rhythm));
                        rhythm = RhythmList[undo.index];
                    }

                    _undoManager.RecordRhythmRedo(undo.rhythm, undo.undoAction, undo.index, undo.lastSelectedNote);

                    rhythmListView.Select(undo.index);
                    _timeline.SetRhythm(rhythm);
                    _timeline.SetSelectedNoteIndex(undo.lastSelectedNote);

                }
                else
                {

                    RhythmList[undo.index] = undo.rhythm;
                    rhythm = RhythmList[undo.index];
                    _undoManager.RecordRhythmRedo(undo.rhythm, undo.undoAction, undo.index, undo.lastSelectedNote);

                    rhythmListView.Select(undo.index);
                    _timeline.SetRhythm(rhythmListView.Current);
                    _timeline.SetSelectedNoteIndex(undo.lastSelectedNote);

                }

                _undoManager.RemoveLastUndo();
            }

        }


        private void DrawListButtons(float maxWidth = 200)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(maxWidth));
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(addContentButton, GUILayout.Width(28), GUILayout.Height(20)))
            {
                AddRhythm("New Rhythm " + (RhythmList.Count + 1));
                rhythmListView.Select(RhythmList.Count - 1);
               
                //_timeline.SetRhythm(rhythmListView.selectedItem);
                //_timeline.SetSelectedNoteIndex(-1);
                _undoManager.RecordRhythm(rhythmListView.Current, "Add New Rhythm", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
            }

            if (rhythmListView.HasSelection)
            {
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = false;
            }


            if (GUILayout.Button(copyContentButton, GUILayout.Width(28), GUILayout.Height(20)))
            {
                if (rhythmListView.HasSelection)
                {
                    Rhythm clone = new Rhythm(rhythmListView.Current);
                    clone.Name = string.Concat(clone.Name, " (Copy)");
                    RhythmList.Insert(rhythmListView.SelectedIndex + 1, clone);

                    rhythmListView.Select(rhythmListView.SelectedIndex + 1);

                    _timeline.Stop();
                    _timeline.SetRhythm(clone);
                    _timeline.SetSelectedNoteIndex(-1);
                    _undoManager.RecordRhythm(clone, "Duplicate Rhythm", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);

                }
            }

            GUILayout.EndHorizontal();

            if (GUILayout.Button(deleteContentButton, GUILayout.Width(28), GUILayout.Height(20)))
            {
                if (rhythmListView.HasSelection)
                {
                    int value = rhythmListView.SelectedIndex;
                    _undoManager.RecordRhythm(rhythmListView.Current, "Remove Rhythm", value, _timeline.GetSelectedNoteIndex(), true);
                    DeleteRhythmAt(value);

                    if (_timeline.IsPlaying())
                        _timeline.Stop();

                    int selection = rhythmListView.SelectedIndex;
                    if (selection >= RhythmList.Count)
                        selection = RhythmList.Count - 1;

                    if (RhythmList.Count == 0)
                    {
                        rhythmListView.UnSelect();
                    }
                    else
                    {
                        rhythmListView.Select(selection);
                    }

                    if (selection != -1)
                    {
                        _timeline.SetRhythm(rhythmListView.Current);
                        _timeline.SetSelectedNoteIndex(-1);
                    }
                }
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }


        public void DrawListBox(Rect pageArea, Rect layoutArea, float ItemHeight)
        {

            // GUILayout.BeginArea(layoutArea);
            // GUI.BeginGroup(pageArea, "");

            
            if (rhythmListView.List == null)
            {
                rhythmListView.List = RhythmList;
            }
            rhythmListView.Draw(200, 200);
            DrawListButtons();
            //GUI.Box(layoutArea, "");



            //int count = RhythmList != null ? RhythmList.Count : 0;
            //totalsize = (ItemHeight * count);
            //float _y = -(ItemHeight * vSbarValue);

            //// Loop through to draw the entries and check for selection.
            //for (int i = 0; i < count; i++)
            //{
            //    //Get the list entry's name
            //    _s = RhythmList[i].Name;
            //    //Get the selection's area.
            //    Rect _entryBox;
            //    if (totalsize > listBox.height)
            //    {
            //        _entryBox = new Rect(1, _y + 1, listBox.width - 16, ItemHeight - 2);
            //    }
            //    else
            //    {
            //        _entryBox = new Rect(1, _y + 1, listBox.width - 2, ItemHeight - 1);
            //    }

            //    if (_entryBox.Contains(_mpos) && _mouseChanged)
            //    {
            //        _lastselected = _selected;
            //        _selected = i;

            //        if (_timeline.IsPlaying())
            //            _timeline.Stop();

            //        m_rhythm = RhythmList[i];
            //        _timeline.SetRhythm(m_rhythm);
            //        _timeline.SetSelectedNoteIndex(-1);
            //        _mouseChanged = false;
            //    }

            //    if (_selected == i)
            //    {
            //        GUI.color = Colors.Selection;
            //        GUI.DrawTexture(_entryBox, Icons.Pixel);
            //        GUI.color = Color.white;
            //    } 
            //    else
            //    {
            //        GUI.color = EditorStyles.label.normal.textColor;
            //    }


            //    GUI.Label(_entryBox, _s, listItemStyle);


            //    _y += ItemHeight;
            //}

            //GUI.color = Color.white;

            //if (totalsize >= listBox.height)
            //{
            //    float viewableRatio = listBox.height / totalsize;
            //    vSbarValue = GUI.VerticalScrollbar(
            //        new Rect(Area.width - 15, 0, 30, listBox.height),
            //        vSbarValue,
            //        RhythmList.Count * viewableRatio,
            //        0, 
            //        RhythmList.Count);
            //}
            //else
            //{
            //    vSbarValue = 0;
            //}


            //if (Event.current != null && count > 0)
            //{
            //    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            //    {
            //        _mpos = Event.current.mousePosition;
            //        _mouseChanged = true;
            //    }
            //    else if (Event.current.type == EventType.ScrollWheel)
            //    {
            //        if (listBox.Contains(Event.current.mousePosition))
            //        {
            //            if (Event.current.delta.y > 0)
            //            {
            //                vSbarValue += 1;
            //                if (vSbarValue >= RhythmList.Count)
            //                    vSbarValue = RhythmList.Count;
            //            }
            //            else if (Event.current.delta.y < 0)
            //            {
            //                vSbarValue -= 1;
            //                if (vSbarValue <= 0)
            //                    vSbarValue = 0;
            //            }
            //        }

            //    }
            //}

            //



            //GUI.EndGroup();
            // GUILayout.EndArea();



        }

        public void DrawRhythmSettings(Rect ritArea)
        {
            float boxy;
            float boxw;
            float totalsizebox;
            float ylast;

            if (rhythmListView.HasSelection)
            {
                Rhythm rhythm = rhythmListView.Current;

                GUI.color = Color.white;
                GUILayout.BeginArea(ritArea, "");
                if (_timeline.IsPlaying())
                    GUI.enabled = false;
                else
                    GUI.enabled = true;


                GUILayout.Label("Rhythm Settings", EditorStyles.boldLabel);
                EditorGUI.BeginChangeCheck();
                string str = EditorGUILayout.TextField("Name", rhythm.Name, GUILayout.Width(ritArea.width));
                if (EditorGUI.EndChangeCheck())
                {
                    _undoManager.RecordRhythm(rhythm, "Set Name", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                    rhythm.Name = str;
                }

                GUI.enabled = true;

                EditorGUI.BeginChangeCheck();
                float t = EditorGUILayout.FloatField("Tempo (BPM)", rhythm.BPM, GUILayout.Width(ritArea.width));
                if (EditorGUI.EndChangeCheck())
                {
                    if (t < 0.1f)
                    {
                        t = 0.1f;
                    }

                    _undoManager.RecordRhythm(rhythm, "Set BPM", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                    rhythm.BPM = t;
                }

                GUI.enabled = true;

                GUILayout.Label("Track Editor:", EditorStyles.boldLabel);

                _timeline.Draw(ritArea.width, 40);


                if (_timeline.IsPlaying())
                    GUI.enabled = false;
                else
                    GUI.enabled = true;


                if (_timeline.GetSelectedNoteIndex() >= 0 && rhythm.Count > 0)
                {

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Button("Add Note", GUILayout.Width(100)))
                    {
                        _undoManager.RecordRhythm(rhythm, "Add Rhythm Note", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                        if (_noteList.Count == 0)
                        {
                            rhythm.AppendNote(0, 0.5f, true);
                        }
                        else
                        {
                            rhythm.AppendNote(0, 0.5f, false);
                        }
                        _timeline.SetSelectedNoteIndex(rhythm.Count - 1);
                    }

                    if (GUILayout.Button("Insert Note", GUILayout.Width(100)))
                    {

                        bool pause = false;

                        if (_noteList.Count == 0)
                            pause = true;

                        Note note = new Note(0.5f, pause, 0);

                        _undoManager.RecordRhythm(rhythm, "Insert Note", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                        rhythm.InsertNoteAt(_timeline.GetSelectedNoteIndex(), note);
                        _timeline.SetSelectedNoteIndex(_timeline.GetSelectedNoteIndex());
                    }

                    if (GUILayout.Button("Duplicate", GUILayout.Width(100)))
                    {
                        _undoManager.RecordRhythm(rhythm, "Duplicate Note", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);

                        Note note = _timeline.GetSelectedNote();
                        note = note.Clone();

                        rhythm.InsertNoteAt(_timeline.GetSelectedNoteIndex(), note);
                        _timeline.SetSelectedNoteIndex(_timeline.GetSelectedNoteIndex() + 1);
                    }

                    if (GUILayout.Button("Remove", GUILayout.Width(100)))
                    {

                        int selDel = _timeline.GetSelectedNoteIndex();
                        _undoManager.RecordRhythm(rhythm, "Remove Note", rhythmListView.SelectedIndex, selDel, true);
                        rhythm.RemoveNote(selDel);

                        if (selDel >= rhythm.Count - 1)
                            selDel = rhythm.Count - 1;

                        _timeline.SetSelectedNoteIndex(selDel);

                    }

                    if (_timeline.GetSelectedNoteIndex() != 0)
                    {
                        GUI.enabled = true;
                    }
                    else
                    {
                        GUI.enabled = false;
                    }

                    GUIContent content = new GUIContent(Icons.LeftArrow, "Move note to left");
                    if (GUILayout.Button(content, GUILayout.Width(24), GUILayout.Height(18)))
                    {
                        _undoManager.RecordRhythm(rhythm, "Move note Left", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                        int index = _timeline.GetSelectedNoteIndex();
                        rhythm.SwapNote(index, index - 1);
                        _timeline.SetSelectedNoteIndex(index - 1);


                    }

                    if (_timeline.GetSelectedNoteIndex() < rhythm.Count - 1)
                    {
                        GUI.enabled = true;
                    }
                    else
                    {
                        GUI.enabled = false;
                    }

                    GUIContent content2 = new GUIContent(Icons.RightArrow, "Move note to right");
                    if (GUILayout.Button(content2, GUILayout.Width(24), GUILayout.Height(18)))
                    {
                        _undoManager.RecordRhythm(rhythm, "Move note Right", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                        int index = _timeline.GetSelectedNoteIndex();
                        rhythm.SwapNote(index, index + 1);
                        _timeline.SetSelectedNoteIndex(index + 1);
                    }

                    GUI.enabled = true;

                    GUILayout.EndHorizontal();

                }
                else
                {

                    if (GUILayout.Button("Add Note", GUILayout.Width(100)))
                    {
                        _undoManager.RecordRhythm(rhythm, "Add Rhythm Note", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                        if (_noteList.Count == 0)
                        {
                            rhythm.AppendNote(0, 0.5f, true);
                        }
                        else
                        {
                            rhythm.AppendNote(0, 0.5f, false);
                        }
                        _timeline.SetSelectedNoteIndex(rhythm.Count - 1);
                    }


                }

                GUI.enabled = true;

                if (_timeline.GetSelectedNoteIndex() >= 0 && rhythmListView.Current.Count > 0)
                {

                    float h = ritArea.height - GUILayoutUtility.GetLastRect().y - ritArea.y;

                    Rect baseRect = new Rect(GUILayoutUtility.GetLastRect().x + 5, GUILayoutUtility.GetLastRect().y + 24 + 5, ritArea.width - 1, ritArea.height - GUILayoutUtility.GetLastRect().y - ritArea.y);

                    // TextureUtility.DrawBox(new Rect(GUILayoutUtility.GetLastRect().x, GUILayoutUtility.GetLastRect().y + 24, ritArea.width - 1, h), Color.black, ref m_pixelSelect, 1);

                    GUI.BeginGroup(baseRect);
 
                    GUI.Label(new Rect(0, 0, 100, 16), "Note Settings:", EditorStyles.boldLabel);
                    GUI.Label(new Rect(baseRect.width - 80, 0, 100, 16), "Index: " + _timeline.GetSelectedNoteIndex().ToString(), EditorStyles.label);

                    bool checkerNote = true;
                    bool oldOption = rhythm.Notes[_timeline.GetSelectedNoteIndex()].isRest;

                    if (oldOption)
                        checkerNote = false;

                    checkerNote = GUI.Toggle(new Rect(0, 24, 100, 16), checkerNote, "Note");
                    checkerNote = GUI.Toggle(new Rect(100, 24, 100, 16), !checkerNote, "Rest");

                    if (checkerNote != oldOption)
                    {

                        bool opt = true;
                        if (oldOption == false)
                            opt = true;
                        else if (oldOption == true)
                            opt = false;

                        _undoManager.RecordRhythm(rhythm, "Toggle Note-Rest", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                        Note nt = rhythm.Notes[_timeline.GetSelectedNoteIndex()];
                        nt.isRest = opt;
                        rhythm.ReplaceNote(_timeline.GetSelectedNoteIndex(), nt);
                    }

                    GUI.Label(new Rect(0, (24 * 2), 100, 16), "Value:", EditorStyles.boldLabel);

                    string[] strDur2 = { "8", "4", "2", "1", "1/2", "1/4", "1/8", "1/16", "1/32", "1/64", "1/128", "1/256" };

                    float dur = rhythm.Notes[_timeline.GetSelectedNoteIndex()].duration;
                    int selDur = DurationToIndex(dur);
                    int oldSelect = selDur;


                    selDur = GUI.SelectionGrid(new Rect(0, (24 * 3), 256, 18 * 3), selDur, strDur2, 4);
                    if (oldSelect != selDur)
                    {
                        _undoManager.RecordRhythm(rhythm, "Set note Duration", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                        dur = IndexToDuration(selDur);
                        Note nt = rhythm.Notes[_timeline.GetSelectedNoteIndex()];
                        nt.duration = dur;
                        rhythm.ReplaceNote(_timeline.GetSelectedNoteIndex(), nt);
                    }

                    if (rhythm.Notes[_timeline.GetSelectedNoteIndex()].isRest)
                        GUI.enabled = false;
                    else
                        GUI.enabled = true;

                    ylast = (24 * 3 + 18 * 3) + 5;

                    GUI.Label(new Rect(0, ylast, 100, 16), "Layout:", EditorStyles.boldLabel);


                    boxw = baseRect.width * 0.5f;
                    ylast += 24;

                    Rect notelistBox = new Rect(0, ylast, boxw, baseRect.height - ylast - 3);
                    totalsizebox = 18 * _noteList.Count;
                    boxy = -(18 * vSbarValueNl);

                    GUI.Box(notelistBox, "");

                    GUI.BeginGroup(notelistBox);


                    int sel = _timeline.GetSelectedNoteIndex();

                    for (int i = 0; i < _noteList.Count; i++)
                    {
                        Rect labelRect;

                        if (totalsizebox > notelistBox.height)
                        {
                            labelRect = new Rect(0, boxy, boxw - 16, 18);
                        }
                        else
                        {
                            labelRect = new Rect(0, boxy, boxw - 2, 18);
                        }

           
                        if (labelRect.Contains(_mpos2) && _mouseChanged2)
                        {
                            _undoManager.RecordRhythm(rhythm, "Set note layout", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
                            Note nt = rhythm.Notes[_timeline.GetSelectedNoteIndex()];
                            nt.layoutIndex = i;
                            rhythm.ReplaceNote(_timeline.GetSelectedNoteIndex(), nt);
                            _mouseChanged2 = false;
                        }

                        if (i == rhythm.Notes[sel].layoutIndex)
                        {
                            GUI.color = Colors.Selection;
                            GUI.DrawTexture(labelRect, Icons.Pixel);
                            GUI.color = Color.white;
                            GUI.Label(new Rect(5 + 20, boxy + 1, boxw - 25 - 1, 16), _noteList[i].Clip.name, listItemStyle);
                        }
                        else
                        {
                            GUI.color = Color.white;
                            GUI.Label(new Rect(5 + 20, boxy + 1, boxw - 25 - 1, 16), _noteList[i].Clip.name, listItemStyle);
                        }

                        Color noteColor = _noteList[i].Color;
                        noteColor.a = 1f;
                        GUI.color = noteColor;
                        GUI.Box(new Rect(5 + 1 + 1, boxy + 1, 16, 16), "");
                        GUI.DrawTexture(new Rect(5 + 1 + 2, boxy + 2, 14, 14), Icons.Pixel);
                        GUI.color = Color.white;

                        boxy += 18;

                    }

                    GUI.backgroundColor = Color.white;



                    if (totalsizebox >= notelistBox.height)
                    {
                        float viewableRatio = notelistBox.height / totalsizebox;
                        vSbarValueNl = GUI.VerticalScrollbar(
                            new Rect(notelistBox.width - 15, 0, 30, notelistBox.height),
                            vSbarValueNl,
                            _noteList.Count * viewableRatio,
                            0,
                            _noteList.Count);

                       
                    }

                    GUI.enabled = true;

                    if (Event.current != null)
                    {
                        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                        {
                            _mpos2 = Event.current.mousePosition;
                            _mouseChanged2 = true;
                        }
                        else if (Event.current.type == EventType.ScrollWheel)
                        {

                            if (notelistBox.Contains(Event.current.mousePosition) && totalsizebox >= notelistBox.height)
                            {
                                if (Event.current.delta.y > 0)
                                {

                                    vSbarValueNl += 1f;
                                    if (vSbarValueNl >= (float)_noteList.Count - 1)
                                        vSbarValueNl = (float)_noteList.Count - 1;

                                }
                                else if (Event.current.delta.y < 0)
                                {

                                    vSbarValueNl -= 1;
                                    if (vSbarValueNl <= 0)
                                        vSbarValueNl = 0;

                                }
                            }

                        }

                        if (Event.current.type == EventType.KeyDown)
                        {
                            if (Event.current.control && Event.current.keyCode == KeyCode.Z)
                            {

                                PerformUndo();
                            }
                        }
                    }



                    GUI.EndGroup();
                    GUI.EndGroup();

                }


                GUILayout.EndArea();


            }


        }

        int DurationToIndex(float duration)
        {

            if (duration == 8f)
            {
                return 0;
            }
            else if (duration == 4f)
            {
                return 1;
            }
            else if (duration == 2f)
            {
                return 2;
            }
            else if (duration == 1f)
            {
                return 3;
            }
            else if (duration == 0.5f)
            {
                return 4;
            }
            else if (duration == 0.25f)
            {
                return 5;
            }
            else if (duration == 0.125f)
            {
                return 6;
            }
            else if (duration == 1f / 16f)
            { // 0.0625f
                return 7;
            }
            else if (duration == 1f / 32f)
            { //0.03125f
                return 8;
            }
            else if (duration == 1f / 64f)
            { //0.015625f
                return 9;
            }
            else if (duration == 1f / 128f)
            { // 0.0078125f 
                return 10;
            }
            else if (duration == 1f / 256f)
            { // 0.00390625
                return 11;
            }

            return -1;

        }

        float IndexToDuration(int index)
        {

            if (index == 0)
                return 8f;
            else if (index == 1)
                return 4f;
            else if (index == 2)
                return 2f;
            else if (index == 3)
                return 1f;
            else if (index == 4)
                return 0.5f;
            else if (index == 5)
                return 0.25f;
            else if (index == 6)
                return (1f / 8f);
            else if (index == 7)
                return (1f / 16f);
            else if (index == 8)
                return (1f / 32f);
            else if (index == 9)
                return (1f / 64f);
            else if (index == 10)
                return (1f / 128f);
            else if (index == 11)
                return (1f / 256f);

            return -1;

        }

    }

}

