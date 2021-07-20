using UnityEngine;
using UnityEditor;
using RhythmosEditor.UI;
using RhythmosEngine;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class Timeline
    {
        // Constants
        private const float rulerBorder = 1;
        private const float trackHeight = 48;
        private const float rulerHeight = 20;

        // Playhead control

        private float playheadPositionX;
        private float playheadPositionY;
        private int playHeadHotControl;

        // Scoll and zoom
        private float scrollbarPosition = 0;
        private float zoomFactor = 0.5f;

        // Rects
        private Rect rulerRect;
        private Rect selectedNoteRect;
        private Rect timelineRect;

        // Controllers
        private Player player;
        private EditController controller;


        public Timeline(EditController controller, Player player)
        {
            this.player = player;
            this.controller = controller;
            playheadPositionX = DrawCommons.RulerOffset;
        }

        internal void Draw(Rect rect)
        {

            rulerRect = Ruler(rect.width);

            Rect currentTimelineRect = GUILayoutUtility.GetRect(rect.width, trackHeight + 1, GUILayout.Width(rect.width));
            if (Event.current.type == EventType.Repaint)
            {
                timelineRect = currentTimelineRect;
            }

            Rect viewableArea = currentTimelineRect;
            viewableArea.y = 0;

            #region Track Background
            GUI.color = (EditorGUIUtility.isProSkin) ? Colors.IconActive : Colors.Gray(0.7f);
            GUI.DrawTexture(currentTimelineRect, Icons.Pixel);
            GUI.color = Color.white;
            #endregion

            #region Track timeline
            GUI.BeginGroup(currentTimelineRect);
            Rhythm rhythm = controller.Rhythm;
            AudioReference audioRef = null;
            float noteSize, totalSize = 0, duration = 0;
            int invalidLevel = 0;
            int noteCount = rhythm == null ? 0 : rhythm.Count;
            Rect noteRect = new Rect(DrawCommons.RulerOffset - scrollbarPosition, 0, 0, currentTimelineRect.height - 1);
            Rect separator = noteRect;
            separator.width = 1;
            separator.height = currentTimelineRect.height - 1;
            Color noteColor = Color.white;
            for (int i = 0; i < noteCount; i += 1)
            {
                // Compute note width
                Note current = rhythm.Notes[i];

                noteSize = Mathf.Max(2f, Mathf.Ceil(DrawCommons.NoteSize * current.duration * controller.BpmInSeconds));
                duration += current.duration;
                noteRect.width = noteSize;

                // Check if this note is the selected one
                if (controller.SelectedNoteIndex == i)
                {
                    selectedNoteRect = noteRect;
                    selectedNoteRect.x -= 1;
                    selectedNoteRect.width += 1;
                }

                if (!noteRect.Overlaps(viewableArea))
                {
                    noteRect.x += noteSize;
                    totalSize += noteSize;
                    continue;
                }


                if (noteRect.Contains(Event.current.mousePosition) && GUIEvents.MouseDown)
                {
                    controller.SelectNote(i);
                    Event.current.Use();
                }



                // If is not a rest get note color
                if (!current.isRest)
                {
                    audioRef = controller.GetAudioReference(current.layoutIndex);
                    invalidLevel = GetInvalidLevel(audioRef);
                    if (invalidLevel == 0)
                    {
                        noteColor = audioRef.Color;
                        noteColor.a = 1f;
                    }
                }

                // If is a rest or invalid note
                if (current.isRest || invalidLevel > 0)
                {
                    noteColor = EditorGUIUtility.isProSkin ? Colors.DarkGray : Colors.ToggleOff;
                    noteColor.a = 0.5f;
                }

                // Draw note background
                GUI.color = noteColor;
                GUI.DrawTexture(noteRect, Icons.TimelineBg);
                if (invalidLevel == 2 && !current.isRest)
                {
                    GUI.color = audioRef.Color;
                    GUI.DrawTexture(new Rect(noteRect.x, noteRect.y + trackHeight - 3, noteRect.width, 3), Icons.Pixel);
                }

                // Playing background
                if (player.IsPlaying && player.IndexNote - 1 == i && player.NoteHightlightTime > 0)
                {
                    GUI.color = Colors.Gray(0, 0.4f);
                    GUI.DrawTexture(new Rect(noteRect.x, noteRect.y, noteRect.width, noteRect.height - 3), Icons.Pixel);
                }

                // Label
                DrawNoteLabel(noteRect, current);

                // Draw note separator
                GUI.color = Color.black;
                separator.x = noteRect.x - 1;

                GUI.DrawTexture(separator, Icons.Pixel);
                if (i == noteCount - 1)
                {
                    separator.x = noteRect.x + noteSize - 1;
                    GUI.DrawTexture(separator, Icons.Pixel);
                }

                noteRect.x += noteSize;
                totalSize += noteSize;
            }
            player.Duration = duration * controller.BpmInSeconds;


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
                Components.OutlineBox(selectedNoteRect, Colors.Gray(1), 1);
            }


            GUI.EndGroup();

            // Play head
            playheadPositionY = rulerRect.y;
            DrawPlayhead(totalSize);

            #endregion

            GUI.color = Color.white;

            #region Scrollbar & Slider

            float sliderWidth = 48;
            if (rect.width > 300)
            {
                sliderWidth = Mathf.Clamp(48 * rect.width / 300, 48, 100);
            }


            Rect scrollbarRect = GUILayoutUtility.GetRect(rect.width, 13);

            Rect scrollRect = scrollbarRect;
            scrollRect.width -= sliderWidth;

            float viewableRatio = 1;
            float totalViewableScroll = totalSize + DrawCommons.RulerOffset;
            if (controller.HasSelection && totalViewableScroll > timelineRect.width)
            {

                totalViewableScroll += DrawCommons.NoteBaseSize;
                viewableRatio = timelineRect.width / totalViewableScroll;
                GUI.enabled = true;


                // Move scrollbar when is playing
                if (player.IsPlaying)
                {
                    Rect viewableRect = timelineRect;
                    viewableRect.x = scrollbarPosition;
                    if (!viewableRect.Contains(new Vector2(playheadPositionX, viewableRect.y + 1)))
                    {
                        scrollbarPosition = Mathf.Clamp(playheadPositionX, 0, totalViewableScroll - (viewableRatio * totalViewableScroll));
                    }
                }


            }
            else
            {
                GUI.enabled = false;
                scrollbarPosition = 0;
            }

            scrollbarPosition = GUI.HorizontalScrollbar(scrollRect, scrollbarPosition, viewableRatio * totalViewableScroll, 0, totalViewableScroll);
            scrollRect.width = sliderWidth;
            scrollRect.x = scrollbarRect.x + scrollbarRect.width - sliderWidth;
            scrollRect.y -= 2;
            scrollRect.height = 16;


            // Zoom slider
            GUI.enabled = controller.HasSelection;
            float zoom = GUI.HorizontalSlider(scrollRect, zoomFactor, 0.0f, 1f);
            if (zoomFactor != zoom)
            {
                zoomFactor = zoom;
                if (zoomFactor > 0.5f)
                {
                    controller.Zoom = Mathf.Lerp(1f, 6f, (zoomFactor - 0.5f) / 0.5f);
                }
                else if (zoomFactor < 0.5f)
                {
                    controller.Zoom = Mathf.Lerp(0.2f, 1f, zoomFactor / 0.5f);
                }
                else
                {
                    controller.Zoom = 1f;
                    zoomFactor = 0.5f;
                }
            }

            #endregion


            GUIUtils.SetColor(Colors.EnabledHorizontalLine, Colors.DisabledHorizontalLine);
            Components.HorizontalLine();
            GUIUtils.ResetColor();
        }

        private void DrawPlayhead(float totalSize)
        {
            if (GUIEvents.IsMouseInside(rulerRect) && GUIEvents.MouseDown)
            {
                playHeadHotControl = GUIEvents.SetHotControl(rulerRect);
                player.IsMovingPlayhead = true;
            }

            if (player.IsMovingPlayhead)
            {
                float playHeadX = Mathf.Clamp(Event.current.mousePosition.x + scrollbarPosition, rulerRect.x + DrawCommons.RulerOffset, totalSize + DrawCommons.RulerOffset);
                float playheadPos = (playHeadX - DrawCommons.RulerOffset) / totalSize;
                if (playHeadX != playheadPos)
                {
                    player.Playhead = playheadPos;
                    Repainter.Request();
                }


                if (GUIUtility.hotControl == playHeadHotControl && Event.current.rawType == EventType.MouseUp)
                {
                    player.SetPlayhead(playheadPos);
                    GUIEvents.ReleaseHotControl();
                }
            }

            GUI.color = GUI.enabled ? Color.white : Colors.Gray(1, 0.5f);
            float playheadLineX = DrawCommons.RulerOffset + (player.Playhead * totalSize);
            Rect playheadRect = new Rect(
                    playheadLineX - Icons.PlayHead.width * 0.5f - scrollbarPosition,
                    playheadPositionY,
                    Icons.PlayHead.width,
                    Icons.PlayHead.height);

            GUI.DrawTexture(playheadRect, Icons.PlayHead);

            playheadRect.y += rulerHeight;
            playheadRect.x = playheadLineX - 1f - scrollbarPosition;
            playheadRect.width = 1;
            playheadRect.height = trackHeight;


            GUI.DrawTexture(playheadRect, Icons.Pixel);


            playheadPositionX = playheadLineX;
        }

        private Rect Ruler(float width, float height = 20)
        {

            Rect rect = GUILayoutUtility.GetRect(width, height);

            GUI.color = Colors.Gray(1, 0.1f);
            GUI.DrawTexture(rect, Icons.Pixel);

            #region Outline box
            GUI.color = Colors.EnabledHorizontalLine;
            Rect outRect = rect;
            outRect.y = rect.y + rect.height - rulerBorder;
            outRect.height = rulerBorder;
            GUI.DrawTexture(outRect, Icons.Pixel);
            #endregion

            #region Track intervals
            GUI.BeginClip(rect);

            float bpmInSeconds = controller.BpmInSeconds;
            if (controller.BpmInSeconds <= 0)
            {
                bpmInSeconds = 1;
            }
            float intervalSize = Mathf.Ceil(DrawCommons.NoteSize * bpmInSeconds);
            int istart = Mathf.FloorToInt((DrawCommons.RulerOffset + scrollbarPosition) / intervalSize);
            int iend = Mathf.RoundToInt((width + scrollbarPosition) / intervalSize) + istart + 1;
            int noteInterval = Mathf.RoundToInt(width / intervalSize) + 1;
            float x = DrawCommons.RulerOffset - scrollbarPosition - 1;
            Rect intervalRect = new Rect(x, rect.height - 10, 1, 9);
            Rect semiIntervalRect = intervalRect;
            semiIntervalRect.height = 4;
            semiIntervalRect.y = rect.height - semiIntervalRect.height - 1;

            for (int i = istart; i < iend; i += 1)
            {
                // GUI.color = Color.gray;
                // semiIntervalRect.x = x;
                // for (j = 1; j < 5; j += 1)
                // {
                //     semiIntervalRect.x = x + (DrawCommons.NoteSize * j) - 1;
                //     GUI.DrawTexture(semiIntervalRect, Icons.Pixel);
                // }

                GUI.color = Color.black;
                intervalRect.x = x;
                GUI.DrawTexture(intervalRect, Icons.Pixel);
                // GUI.Label(new Rect(intervalRect.x + 1, intervalRect.y - 8, 64, 14), StringUtils.TimeSecMilli((i - istart) * 0.05f), EditorStyles.miniLabel);

                x += intervalSize;
            }

            GUI.EndClip();
            #endregion

            return rect;
        }

        private static int GetInvalidLevel(AudioReference audioRef)
        {
            if (audioRef == null)
            {
                return 1;
            }
            else
            {
                if (audioRef.Clip != null)
                {
                    return 0;
                }
            }
            return 2;
        }

        private void DrawNoteLabel(Rect noteRect, Note current)
        {
            Color color = Color.white;
            if (noteRect.width > 6)
            {
                if (current.isRest)
                {
                    GUI.color = color;
                    float restOffset = noteRect.width < 20 ? noteRect.width / 2 - 8 : 2;
                    GUI.DrawTexture(new Rect(noteRect.x + restOffset, noteRect.y + trackHeight / 2 - 10, 16, 16), Icons.Rest);
                }
                else
                {
                    AudioReference audioRef = controller.GetAudioReference(current.layoutIndex);

                    string word;
                    if (audioRef == null)
                    {
                        color = Colors.Invalid;
                        word = "[INVALID]";
                    }
                    else
                    {
                        if (audioRef.Clip != null)
                        {
                            word = audioRef.Clip.name;
                        }
                        else
                        {
                            color = Colors.Invalid;
                            word = "[NULL]";
                        }
                    }

                    Components.DropShadowMiniLabel(
                        new Rect(
                            noteRect.x + 2,
                            noteRect.y + trackHeight / 2 - 10,
                            noteRect.width - 4,
                         20),
                        word,
                        color);
                }
            }
        }
    }
}
