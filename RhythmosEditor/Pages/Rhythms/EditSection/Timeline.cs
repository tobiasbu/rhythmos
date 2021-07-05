using RhythmosEditor.UI;
using RhythmosEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class Timeline
    {
        private Player player;
        private EditController controller;
        private const float trackHeight = 48;
        private float scrollbarPosition;

        public Timeline(EditController controller, Player player)
        {
            this.player = player;
            this.controller = controller;
        }

        internal void Draw(Rect rect)
        {
            

            
            Rect timelineRect = GUILayoutUtility.GetRect(rect.width, trackHeight + 1);

            #region Track Background

            GUI.color = Colors.Gray(0.7f);
            GUI.DrawTexture(timelineRect, Icons.Pixel);
            GUI.color = Color.white;
            
            GUI.BeginGroup(timelineRect);

            #endregion


            #region Track timeline

            Rhythm rhythm = controller.Rhythm;
            float noteSize;
            bool invalidNote = false;
            int noteCount = rhythm == null ? 0 : rhythm.Count;
            Rect noteRect = new Rect(DrawCommons.RulerOffset, 0, 0, timelineRect.height - 1);
            Rect selectedNoteRect = noteRect;
            Color noteColor = Color.white;

            for (int i = 0; i < noteCount; i += 1)
            {
                // Compute note width
                Note current = rhythm.Notes[i];
                noteSize = Mathf.Round(TrackDraw.noteBaseSize * current.duration) - 1;
                if (noteSize < 2)
                {
                    noteSize = 2;
                }
                noteRect.width = noteSize;

                // Check if this note is the selected one
                if (controller.SelectedNoteIndex == i)
                {
                    selectedNoteRect = noteRect;
                }

                // If is not a rest get note color
                if (!current.isRest)
                {
                    AudioReference audioRef = controller.GetAudioReference(current.layoutIndex);
                    invalidNote = audioRef == null;
                    if (!invalidNote)
                    {
                        noteColor = audioRef.Color;
                        noteColor.a = 1f;
                    } 
                } 

                // If is a rest or invalid note
                if (current.isRest || invalidNote)
                {
                    noteColor = EditorGUIUtility.isProSkin ? Colors.DarkGray : Colors.ToggleOff;
                    noteColor.a = 0.5f;
                }

                

                // Draw note
                GUI.color = noteColor;
                GUI.DrawTexture(noteRect, Icons.TimelineBg);

                // Draw note separator
                GUI.color = Color.black;
                GUI.DrawTexture(new Rect(noteRect.x + noteSize, noteRect.y, 1, noteRect.height), Icons.Pixel);

                noteRect.x += noteSize + 1;
            }

            #endregion

            #region Note selection

            if (controller.HasNoteSelected)
            {
                EditorStyles.whiteBoldLabel.normal.textColor = Color.white;

                // highlight
                float alpha = player.IsPlaying ? 0.02f : 0.15f;
                GUI.color = Colors.Gray(1, alpha);
                GUI.DrawTexture(selectedNoteRect, Icons.Pixel);
                // hightlight borders
                //alpha = player.IsPlaying ? 0.65f : 0.85f;
                //Components.OutlineBox(selectetNoteRect, GUI.color = Colors.Gray(1, alpha), 1);


                //Components.OutlineBox(new Rect(selectedRect.x - 1, selectedRect.y, selectedRect.width + 1, selectedRect.height), Color.white, 1);
            }


            GUI.EndGroup();

            #endregion

            GUI.color = Color.white;

            
            // Scrollbar
            //float factor = GUI.HorizontalSlider(new Rect(GUILayoutUtility.GetLastRect().x + 1 + width - sliderWidth - 5, rectY + height - 2, sliderWidth, 16), m_zoomFactor, 0.05f, 1f);

            Rect scrollbarRect = GUILayoutUtility.GetRect(rect.width, 13);
            scrollbarPosition = GUI.HorizontalScrollbar(scrollbarRect, 0, 1, 0, 0);
            Components.HorizontalLine(DrawCommons.BoxBottomColor);
        }
    }
}
