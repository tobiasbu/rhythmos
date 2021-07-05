using UnityEditor;
using UnityEngine;
using RhythmosEditor.Pages.Rhythms;
using RhythmosEditor.UIComponents;
using RhythmosEditor.Utils;
using RhythmosEditor.UI;

namespace RhythmosEditor.Pages
{
    internal class PlayerToolbar
    {
        // Refs
        public Player player;
        public EditController controller;


        // GUI items
        private GUIContent playIcon;
        private GUIContent goToStartIcon;
        private GUIContent goToEndIcon;
        private GUIContent goToNextIcon;
        private GUIContent goToPreviousIcon;
        private GUIContent loopIcon;
        private GUIContent muteIcon;
        private GUIContent metronomeIcon;

        // GUI variables
        private const float toolbarHeight = 28;
        private const float buttonHeight = 20;
        private const float timeLabelWidth = 58;

        private Rect boxRect;
        private Rect toolbarButtonsRect;
        private Rect playerRect;

        public void OnLoad()
        {
            if (playIcon == null)
            {
                playIcon = new GUIContent(Icons.Play, "Play");
            }

            if (goToPreviousIcon == null)
            {
                goToPreviousIcon = new GUIContent(Icons.ToPrevious, "Go to preivous note");
            }

            if (goToNextIcon == null)
            {
                goToNextIcon = new GUIContent(Icons.ToNext, "Go to next note");
            }

            if (goToStartIcon == null)
            {
                goToStartIcon = new GUIContent(Icons.ToStart, "Go to start");
            }

            if (goToEndIcon == null)
            {
                goToEndIcon = new GUIContent(Icons.ToEnd, "Go to end");
            }

            if (muteIcon == null)
            {
                muteIcon = new GUIContent(Icons.MuteOn, "Mute on");
            }

            if (loopIcon == null)
            {
                loopIcon = new GUIContent(Icons.Loop, "Enable loop");
            }

            if (metronomeIcon == null)
            {
                metronomeIcon = new GUIContent(Icons.Metronome, "Enable metronome");
            }
        }


        public void Draw(Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                boxRect = GUILayoutUtility.GetLastRect();
            }
            playerRect = new Rect(boxRect.x, boxRect.y + boxRect.height, rect.width, toolbarHeight);

            #region Buttons

            Toolbar.Begin();

            // Go to start
            Toolbar.Button(goToStartIcon);

            Toolbar.Button(goToPreviousIcon);

            // Play button
            Toolbar.Button(playIcon, 28);

            Toolbar.Button(goToNextIcon);

            // Go to end
            Toolbar.Button(goToEndIcon);

            Toolbar.Separator();

            // Loop
            bool loop = Toolbar.Toggle(player.Loop, loopIcon);
            if (loop != player.Loop)
            {
                loopIcon.tooltip = loop ? "Disable loop" : "Enable loop";
                player.Loop = loop;
            }

            // Mute on/off
            bool mute = Toolbar.Toggle(player.Mute, muteIcon);
            if (mute != player.Mute)
            {
                muteIcon.tooltip = mute ? "Unmute" : "Mute";
                muteIcon.image = mute ? Icons.MuteOff : Icons.MuteOn;
                player.Mute = mute;
            }

            // Metronome
            bool metronome = Toolbar.Toggle(player.Metronome, metronomeIcon);
            if (metronome != player.Metronome)
            {
                loopIcon.tooltip = metronome ? "Disable metronome" : "Enable metronome";
                player.Metronome = metronome;
            }

            if (Event.current.type == EventType.Repaint)
            {
                toolbarButtonsRect = GUILayoutUtility.GetLastRect();
            }

            float toolbarButtonsXmax = toolbarButtonsRect.xMax;

            Toolbar.End();

            #endregion

            #region Timer

            Color oldGuiColor = GUI.color;

            float baseX = (timeLabelWidth * 2) + 10 + 4;
            float timerX = playerRect.width - baseX;
            float timerY = playerRect.y + 2;
            if (timerX < toolbarButtonsXmax + 5)
            {
                timerX = toolbarButtonsXmax + 5;
            }
           
            // Current rhtyhm time
            GUIStyle labelStyle = EditorStyles.label;
            GUI.color = player.IsPlaying ? labelStyle.focused.textColor : Color.gray;
            DrawTimer(0, timerX, timerY);

            // Rhythm Duration
            GUI.color = Color.gray;
            DrawTimer(0, timerX + timeLabelWidth + 10, timerY);

            // Separators
            GUI.color = DrawCommons.BoxSideColor;
            GUI.DrawTexture(new Rect(timerX + timeLabelWidth + 5, timerY, 1, buttonHeight), Icons.Pixel);
            GUI.DrawTexture(new Rect(timerX - 5, timerY, 1, buttonHeight), Icons.Pixel);

            GUI.color = oldGuiColor;
            #endregion

        }

        private void DrawTimer(float time, float x, float y)
        {
            Rect labelRect = new Rect(x, y + 2, timeLabelWidth, 16);
            GUI.Label(labelRect, StringUtils.Time(time), EditorStyles.whiteLabel);
        }

    }
}
