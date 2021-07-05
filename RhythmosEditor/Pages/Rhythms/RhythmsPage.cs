using UnityEngine;
using UnityEditor;
using RhythmosEngine;
using RhythmosEditor.Commands;
using RhythmosEditor.Settings;
using RhythmosEditor.UIComponents;
using RhythmosEditor.UI;
using RhythmosEditor.Pages.Rhythms;

namespace RhythmosEditor.Pages
{
    internal class RhythmsPage : IEditorPage
    {
        // GUI items
        private GUIContent addContentButton;
        private GUIContent copyContentButton;
        private GUIContent deleteContentButton;

        // Main GUI elements
        private ListView<Rhythm> rhythmListView;
        private RhythmEditSection editingSection;

        public void OnLoad()
        {
            if (rhythmListView == null)
            {
                rhythmListView = new ListView<Rhythm>
                {
                    onGetItemLabel = (item) => item.Name,
                    onSelectionChange = (rhythm, index) =>
                    {
                        editingSection.OnRhythmSelectionChange(rhythm, index);
                        EditorGUIUtility.editingTextField = false;
                        GUI.FocusControl(null);
                    }
                };
            }

            if (editingSection == null)
            {
                editingSection = new RhythmEditSection();
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

        public void OnPageSelect(RhythmosConfig config)
        {
            if (config.loaded)
            {
                rhythmListView.List = config.RhythmosDatabase.Rhythms;
                rhythmListView.UnSelect();
                
                //player.AudioReferences = config.RhythmosDatabase.AudioReferences;
            }

            editingSection.Setup(rhythmListView, config.loaded ? config.RhythmosDatabase.AudioReferences : null);
        }

        public void OnDraw(Rect pageRect)
        {
            
            #region List section

            rhythmListView.Draw(200, pageRect.height - 34);
            DrawRhythmListButtons();

            #endregion

            #region Rhythm editing

            Rect rhythmRect = pageRect;
            rhythmRect.y = 0;
            rhythmRect.x += 200;
            rhythmRect.width -= rhythmRect.x;

            editingSection.Draw(rhythmRect, rhythmListView.HasSelection);

            #endregion
        }

        private void DrawRhythmListButtons(float maxWidth = 200)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(maxWidth));
            GUILayout.BeginHorizontal();

            if (Components.IconButton(addContentButton))
            {
                UndoRedo.Record(new Commands.RhythmsList.Create(rhythmListView));
            }

            GUI.enabled = rhythmListView.HasSelection;

            if (Components.IconButton(copyContentButton))
            {
                if (rhythmListView.HasSelection)
                {
                    UndoRedo.Record(new Commands.RhythmsList.Duplicate(rhythmListView));
                }
            }

            GUILayout.EndHorizontal();

            if (Components.IconButton(deleteContentButton))
            {
                if (rhythmListView.HasSelection)
                {
                    UndoRedo.Record(new Commands.RhythmsList.Delete(rhythmListView));
                }
            }

            GUILayout.EndHorizontal();
            GUI.enabled = true;
        }
    }
}

