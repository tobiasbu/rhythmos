using UnityEngine;
using RhythmosEngine;
using RhythmosEditor.UIComponents;
using System.Collections.Generic;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class RhythmEditSection
    {
        // Controllers
        private EditController controller;
        private Player player;

        // UI Components
        private BaseSettings baseSettings;
        private PlayerToolbar playerToolbar;
        private NoteToolbar noteToolbar;
        private Timeline timeline;       

        private Rect lastSettingsRect;

        public void Setup(ListView<Rhythm> rhythmListView, List<AudioReference> audioReferences)
        {

            if (controller == null)
            {
                controller = new EditController();
            }
            controller.Setup(rhythmListView, audioReferences);

            if (player == null)
            {
                player = new Player();
            }

            if (baseSettings == null)
            {
                baseSettings = new BaseSettings();
            }
            baseSettings.OnLoad();
            baseSettings.controller = controller;

            if (playerToolbar == null)
            {
                playerToolbar = new PlayerToolbar();
                playerToolbar.OnLoad();
            }
            playerToolbar.player = player;

            if (timeline == null)
            {
                timeline = new Timeline(controller, player);
            }

            if (noteToolbar == null)
            {
                noteToolbar = new NoteToolbar();
                noteToolbar.OnLoad();
            }
            noteToolbar.controller = controller;

        }

        public void OnRhythmSelectionChange(Rhythm rhythm, int index)
        {
            controller.SetRhythm(rhythm, index);
            player.SetRhythm(rhythm);
            baseSettings.OnRhythmChange();
        }

        internal void Draw(Rect rect, bool hasSelection)
        {
            DrawCommons.OnLoad();


            GUI.enabled = hasSelection;

            GUILayout.BeginArea(rect);

            baseSettings.Draw();

            GUILayout.Space(4);

            if (Event.current.type == EventType.Repaint)
            {
                lastSettingsRect = GUILayoutUtility.GetLastRect();
            }

            playerToolbar.Draw(rect);

            float unit = rect.width * 0.1f;
            if (unit < 16)
            {
                unit = 16;
            }
            TrackDraw.noteBaseSize = unit;

            // Ruler
            TrackDraw.Ruler(rect.width);

            timeline.Draw(rect);

            noteToolbar.Draw(rect);

            GUILayout.EndArea();

            float y = lastSettingsRect.y + lastSettingsRect.height + 1;
            DrawCommons.OutlineBox(new Rect(rect.x, y, rect.width, rect.height - y - 4), 1, true);

        }

        
    }
}
