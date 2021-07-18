using UnityEngine;
using UnityEditor;
using RhythmosEditor.UIComponents;
using RhythmosEditor.UI;
using RhythmosEditor.Commands;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class NoteToolbar
    {
        // Refs
        public EditController controller;

        // GUI items
        private GUIContent duplicateNote;
        private GUIContent removeNote;
        private GUIContent addNote;
        private GUIContent moveLeft;
        private GUIContent moveRight;

        public void OnLoad()
        {
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
        }

        public void Draw()
        {

            Toolbar.Begin();

            if (Toolbar.Button(addNote))
            {
                UndoRedo.Record(new Commands.EditRhythm.AddNote(controller, 0.5f));
            }

            GUI.enabled = controller.HasNoteSelected;

            if (Toolbar.Button(duplicateNote))
            {
                UndoRedo.Record(new Commands.EditRhythm.DuplicateNote(controller));
            }

            if (Toolbar.Button(removeNote))
            {
                UndoRedo.Record(new Commands.EditRhythm.RemoveNote(controller));
            }

            Toolbar.Separator();

            GUI.enabled = controller.HasNoteSelected && controller.SelectedNoteIndex > 0;
            if (Toolbar.Button(moveLeft, 26, 20))
            {
                UndoRedo.Record(new Commands.EditRhythm.MoveNote(controller, Commands.Direction.Left));
            }

            GUI.enabled = controller.HasNoteSelected && controller.SelectedNoteIndex < controller.NoteCount - 1;
            if (Toolbar.Button(moveRight, 26, 20))
            {
                UndoRedo.Record(new Commands.EditRhythm.MoveNote(controller, Commands.Direction.Right));
            }

            GUI.enabled = true;
            Toolbar.Separator();
            GUILayout.Space(4);

            if (controller.HasNoteSelected)
            {
                Color oldContentColor = GUI.contentColor;
                GUI.contentColor = EditorStyles.label.normal.textColor;
                Toolbar.Label(controller.SelectedNoteIndex + 1 + " / " + controller.NoteCount);
                GUI.contentColor = oldContentColor;
            }
            Toolbar.End();
        }
    }
}
