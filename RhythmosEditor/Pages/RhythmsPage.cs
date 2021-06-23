using System.Collections.Generic;
using UnityEngine;
using RhythmosEngine;
using RhythmosEditor.Commands;

namespace RhythmosEditor.Pages
{
    internal class RhythmsPage : BaseEditorPage
    {
        // GUI items
        private GUIContent addContentButton;
        private GUIContent copyContentButton;
        private GUIContent deleteContentButton;
        private ListView<Rhythm> rhythmListView;

        // Reference to rhythmList
        private List<Rhythm> rhythmList;

        public RhythmsPage()
        { }

        public override void OnLoad()
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

        public override void OnPageSelect(Config config)
        {
            if (config.loaded)
            {
                rhythmList = config.RhythmosDatabase.Rhythms;
                rhythmListView.List = rhythmList;
                rhythmListView.UnSelect();
            }
        }

        public override void OnDraw(Rect pageRect)
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
                UndoRedo.Record(new Commands.Rhythms.Create(rhythmListView));
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
                    UndoRedo.Record(new Commands.Rhythms.Duplicate(rhythmListView));
                }
            }

            GUILayout.EndHorizontal();

            if (GUIDraw.IconButton(deleteContentButton))
            {
                if (rhythmListView.HasSelection)
                {
                    UndoRedo.Record(new Commands.Rhythms.Delete(rhythmListView));
                }
            }

            GUILayout.EndHorizontal();
            GUI.enabled = true;
        }
    }

 
}
