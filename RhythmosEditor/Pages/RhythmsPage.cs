using System.Collections.Generic;
using UnityEngine;
using RhythmosEngine;

namespace RhythmosEditor.Pages
{
    internal class RhythmsPage : IEditorPage
    {
        // GUI items
        private GUIContent addContentButton;
        private GUIContent copyContentButton;
        private GUIContent deleteContentButton;
        private ListView<Rhythm> rhythmListView;

        // Reference to rhythmList
        private List<Rhythm> rhythmList;

        public IUndoRedoDelegate UndoRedoDelegate => null;

        public void OnLoad()
        {
            if (rhythmListView == null)
            {
                rhythmListView = new ListView<Rhythm>
                {
                    onGetItemLabel = (item) => item.Name
                };
            }

            if (addContentButton == null)
            {
                addContentButton = new GUIContent(Textures.Add, "Create new rhythm");
            }

            if (copyContentButton == null)
            {
                copyContentButton = new GUIContent(Textures.Copy, "Duplicate selected rhythm");
            }

            if (deleteContentButton == null)
            {
                deleteContentButton = new GUIContent(Textures.Delete, "Remove selected rhythm");
            }
        }

        public void OnPageSelect(Config config)
        {
            if (config.loaded)
            {
                rhythmList = config.RhythmosDatabase.Rhythms;
                rhythmListView.List = rhythmList;
                rhythmListView.UnSelect();
            }
        }

        public void OnDraw(Rect pageRect)
        {
            #region List section

            rhythmListView.Draw(200, pageRect.height - 34);
            DrawListButtons();

            #endregion
        }

        private void DrawListButtons(float maxWidth = 200)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(maxWidth));
            GUILayout.BeginHorizontal();

            if (GUIDraw.IconButton(addContentButton))
            {
                int count = rhythmList.Count;
                rhythmList.Add(new Rhythm {
                    Name = "New Rhythm " + (count + 1),
                    BPM = 80
                });
                rhythmListView.Select(count);

                //_timeline.SetRhythm(rhythmListView.selectedItem);
                //_timeline.SetSelectedNoteIndex(-1);
                //_undoManager.RecordRhythm(rhythmListView.Selected, "Add New Rhythm", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);
            }

            if (rhythmListView.HasSelection)
            {
                GUI.enabled = true;
            }
            else
            {
                GUI.enabled = false;
            }


            if (GUIDraw.IconButton(copyContentButton))
            {
                if (rhythmListView.HasSelection)
                {
                    Rhythm clone = new Rhythm(rhythmListView.Selected);
                    clone.Name = string.Concat(clone.Name, " (Copy)");
                    int insertIndex = rhythmListView.SelectedIndex + 1;
                    rhythmList.Insert(insertIndex, clone);
                    rhythmListView.Select(insertIndex);

                    //_timeline.Stop();
                    //_timeline.SetRhythm(clone);
                    //_timeline.SetSelectedNoteIndex(-1);
                    //_undoManager.RecordRhythm(clone, "Duplicate Rhythm", rhythmListView.SelectedIndex, _timeline.GetSelectedNoteIndex(), true);

                }
            }

            GUILayout.EndHorizontal();

            if (GUIDraw.IconButton(deleteContentButton))
            {
                if (rhythmListView.HasSelection)
                {
                    int selectedIndex = rhythmListView.SelectedIndex;
                    //_undoManager.RecordRhythm(rhythmListView.Selected, "Remove Rhythm", value, _timeline.GetSelectedNoteIndex(), true);
                    rhythmList.RemoveAt(selectedIndex);
                    //_timeline.Stop();

                    int count = rhythmList.Count;
                    if (count == 0)
                    {
                        rhythmListView.UnSelect();
                    }
                    else
                    {
                        if (selectedIndex >= count)
                        {
                            selectedIndex = count - 1;
                        }

                        rhythmListView.Select(selectedIndex);
                    }

                    //if (selection != -1)
                    //{
                    //    _timeline.SetRhythm(rhythmListView.Selected);
                    //    _timeline.SetSelectedNoteIndex(-1);
                    //}
                }
            }

            GUILayout.EndHorizontal();
            GUI.enabled = true;
        }
    }
}
