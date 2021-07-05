using UnityEngine;
using RhythmosEditor.UIComponents;
using RhythmosEditor.UI;
using RhythmosEditor.Commands;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class NoteToolbar
    {
        // Refs
        //public Player player;
        public EditController controller;

        // GUI items
        private GUIContent duplicateNote;
        private GUIContent removeNote;
        private GUIContent addNote;
        private GUIContent moveLeft;
        private GUIContent moveRight;

        readonly string[] beatsValue = { "8", "4", "2", "1", "1/2", "1/4", "1/8", "1/16", "1/32", "1/64", "1/128" };

        private Rect selectedNoteBox;
        private GUIStyle uIStyle;
       

        public void OnLoad() {
            if (addNote == null)
            {
                addNote = new GUIContent(Icons.AddNote, "Add note");
            }

            if (duplicateNote == null)
            {
                duplicateNote = new GUIContent(Icons.Duplicate, "Duplicate selected note");
            }

            if (removeNote == null)
            {
                removeNote = new GUIContent(Icons.Delete, "Remove selected note");
            }

            if (moveLeft == null)
            {
                moveLeft = new GUIContent(Icons.LeftArrow, "Move note to left");
            }

            if (moveRight == null)
            {
                moveRight = new GUIContent(Icons.RightArrow, "Move note to right");
            }

            //if (audioListView == null)
            //{
            //    audioListView = new ListView<AudioReference>();
            //}
      
        }

        public void Draw(Rect rhythmRect) {
            Toolbar.Begin();

            //if (GUIDraw.IconButton("Add Note", GUILayout.Width(100)))
            //{
            //    //_undoManager.RecordRhythm(rhythm, "Add Rhythm Note", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
            //    //if (_noteList.Count == 0)
            //    //{
            //    //    rhythm.AppendNote(0, 0.5f, true);
            //    //}
            //    //else
            //    //{
            //    //    rhythm.AppendNote(0, 0.5f, false);
            //    //}
            //    //_timeline.SetSelectedNoteIndex(rhythm.Count - 1);
            //}

            if (Toolbar.Button(addNote))
            {
                UndoRedo.Record(new Commands.EditRhythm.AddNote(controller, 0.5f));
            }

            GUI.enabled = controller.HasNoteSelected;

            //bool pause = false;

            //if (_noteList.Count == 0)
            //    pause = true;

            //Note note = new Note(0.5f, pause, 0);

            //_undoManager.RecordRhythm(rhythm, "Insert Note", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
            //rhythm.InsertNoteAt(_timeline.GetSelectedNoteIndex(), note);
            //_timeline.SetSelectedNoteIndex(_timeline.GetSelectedNoteIndex());


            Toolbar.Button(duplicateNote);
            //_undoManager.RecordRhythm(rhythm, "Duplicate Note", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);

            //Note note = _timeline.GetSelectedNote();
            //note = note.Clone();

            //rhythm.InsertNoteAt(_timeline.GetSelectedNoteIndex(), note);
            //_timeline.SetSelectedNoteIndex(_timeline.GetSelectedNoteIndex() + 1);

            Toolbar.Button(removeNote);
            //int selDel = _timeline.GetSelectedNoteIndex();
            //_undoManager.RecordRhythm(rhythm, "Remove Note", rhythmListView.SelectedIndex, selDel, true);
            //rhythm.RemoveNote(selDel);

            //if (selDel >= rhythm.Count - 1)
            //    selDel = rhythm.Count - 1;

            //_timeline.SetSelectedNoteIndex(selDel);


            Toolbar.Separator();


            //if (_timeline.GetSelectedNoteIndex() != 0)
            //{
            //    GUI.enabled = true;
            //}
            //else
            //{
            //    GUI.enabled = false;
            //}

            Toolbar.Button(moveLeft, 26, 20);
            //_undoManager.RecordRhythm(rhythm, "Move note Left", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
            //int index = _timeline.GetSelectedNoteIndex();
            //rhythm.SwapNote(index, index - 1);
            //_timeline.SetSelectedNoteIndex(index - 1);

            //if (_timeline.GetSelectedNoteIndex() < rhythm.Count - 1)
            //{
            //    GUI.enabled = true;
            //}
            //else
            //{
            //    GUI.enabled = false;
            //}

            Toolbar.Button(moveRight, 26, 20);
            //_undoManager.RecordRhythm(rhythm, "Move note Right", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
            //int index = _timeline.GetSelectedNoteIndex();
            //rhythm.SwapNote(index, index + 1);
            //_timeline.SetSelectedNoteIndex(index + 1);
        

            GUI.enabled = true;

            Toolbar.End();

   
                //selectedNoteBox = GUILayoutUtility.GetRect(100, 48, GUILayout.ExpandWidth(true));
           
            GUI.color = Color.white;

            Components.OutlineBox(selectedNoteBox, Color.gray);
            //GUI.BeginGroup(selectedNoteBox);
            //selectedNoteBox.y = 0;
            //selectedNoteBox.x = 0;
            //GUILayout.BeginArea(selectedNoteBox, "-----a", EditorStyles.helpBox);
           

            //GUILayout.BeginHorizontal();
            //EditorGUILayout.SelectableLabel("Is Note");

            //GUILayout.EndHorizontal();


            //GUILayout.EndArea();
            //GUI.EndGroup();
            GUILayout.SelectionGrid(0, beatsValue, 11);

            //splitView.BeginSplitView();
            //audioListView.Draw();
            //splitView.Split();
            //GUILayout.Label("ha");
            //splitView.EndSplitView();

        }

    }
}
