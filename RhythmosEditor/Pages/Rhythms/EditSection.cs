using UnityEngine;
using RhythmosEngine;
using RhythmosEditor.UIComponents;
using RhythmosEditor.Settings;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class EditSection
    {

        // UI Components
        private BaseSettings baseSettings;
        private PlayerToolbar playerToolbar;
        private NoteToolbar noteToolbar;
        private NoteSettings noteSettings;
        private AudioList audioList;
        private Timeline timeline;

        private Rect lastSettingsRect;

        // Controllers
        private EditController controller;

        internal Player Player { set; get; }

        public void Setup(ListView<Rhythm> rhythmListView, RhythmosConfig config)
        {

            if (controller == null)
            {
                controller = new EditController();
            }
            controller.Setup(rhythmListView, config);

            if (Player == null)
            {
                Player = new Player();
            }
            Player.controller = controller;
            controller.player = Player;

            if (baseSettings == null)
            {
                baseSettings = new BaseSettings();
            }
            baseSettings.OnLoad();
            baseSettings.player = Player;
            baseSettings.controller = controller;

            if (playerToolbar == null)
            {
                playerToolbar = new PlayerToolbar();
                playerToolbar.OnLoad();
            }
            playerToolbar.controller = controller;
            playerToolbar.player = Player;

            if (timeline == null)
            {
                timeline = new Timeline(controller, Player);
            }

            if (noteToolbar == null)
            {
                noteToolbar = new NoteToolbar();
                noteToolbar.OnLoad();
            }
            noteToolbar.controller = controller;

            if (noteSettings == null)
            {
                noteSettings = new NoteSettings();
            }
            noteSettings.controller = controller;

            if (audioList == null)
            {
                audioList = new AudioList();
            }
            audioList.Setup(controller, Player);
        }

        public void OnRhythmSelectionChange(Rhythm rhythm, int index)
        {
            Player.Stop();
            controller.SetRhythm(rhythm, index);
            baseSettings.OnRhythmChange();
            audioList.OnRhythmChange();
        }

        internal void Draw(Rect rect, bool hasSelection)
        {

            GUI.enabled = hasSelection;

            GUILayout.BeginArea(rect);

            baseSettings.Draw();

            GUILayout.Space(4);

            if (Event.current.type == EventType.Repaint)
            {
                lastSettingsRect = GUILayoutUtility.GetLastRect();
            }

            playerToolbar.Draw(rect);

            // Rhythm timeline
            DrawCommons.NoteSize = Mathf.Ceil(Mathf.Max(2f, controller.Zoom * DrawCommons.NoteBaseSize));
            DrawCommons.TimelineNoteUnit = Mathf.Max(2f, controller.BpmInSeconds * DrawCommons.NoteSize);
            timeline.Draw(rect);

            // Editing note are
            noteToolbar.Draw();
            noteSettings.Draw(rect);
            audioList.Draw();

            GUILayout.EndArea();

            float y = lastSettingsRect.y + lastSettingsRect.height + 1;
            DrawCommons.OutlineBox(new Rect(rect.x, y, rect.width, rect.height - y), 1, true);
        }
    }
}
