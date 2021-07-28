using System.Collections.Generic;
using UnityEngine;
using RhythmosEngine;
using RhythmosEditor.UIComponents;
using RhythmosEditor.UI;
using RhythmosEditor.Utils;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class AudioList
    {
        private GUIContent previewIcon;

        private SearchFieldComponent searchField;
        private ListView<AudioReference> audioListView;
        internal EditController controller;
        internal Player player;

        internal void Setup(EditController controller, Player player, IList<AudioReference> audioReferences)
        {
            this.controller = controller;
            this.player = player;

            controller.OnNoteSelect -= OnNoteSelect;
            controller.OnNoteSelect += OnNoteSelect;

            if (searchField == null)
            {
                searchField = new SearchFieldComponent() {
                    onInputChanged = this.OnSearchFieldInput
                };

            }

            if (audioListView == null)
            {
                audioListView = new ListView<AudioReference> {
                    onDrawItem = this.OnDrawAudioItem,
                    KeyboardNavigation = false,
                    ItemHeight = 20,
                };

                audioListView.List = audioReferences;
            }

            if (previewIcon == null)
            {
                previewIcon = new GUIContent(Icons.Play, "Play preview");
            }
        }

        public void Draw()
        {
            GUI.enabled = controller.HasSelection && controller.HasAudioClips && controller.HasNoteSelected;

            Toolbar.Begin();

            Toolbar.Label("Audio clip", GUILayout.ExpandWidth(true));
            GUILayout.Space(4);

            searchField.OnToolbarGUI();

            Toolbar.End();

            audioListView.Draw();
        }

        internal void OnRhythmChange()
        {
            if (audioListView == null)
            {
                return;
            }
            audioListView.UnSelect();
            searchField.Clear();
        }

        private bool OnDrawAudioItem(AudioReference item, Rect rect, int index)
        {
            bool result = false;
            Color oldGuiColor = GUI.color;

            GUILayout.BeginHorizontal(GUILayout.Width(rect.width));

            GUILayout.Space(2);

            Rect colorAreaRect = GUILayoutUtility.GetRect(20, audioListView.ItemHeight, GUILayout.Width(audioListView.ItemHeight));
            Rect colorRect = new Rect(colorAreaRect.x + 4, colorAreaRect.y + 4, colorAreaRect.width - 8, colorAreaRect.height - 8);

            GUI.color = GUIUtils.GetColor(item.Color, 1f);
            GUI.Box(new Rect(colorRect.x - 1, colorRect.y - 1, colorRect.width + 2, colorRect.height + 2), "");
            GUI.DrawTexture(colorRect, Icons.Pixel);



            if (item.Clip != null)
            {
                GUI.color = oldGuiColor;
                GUILayout.Label(item.Clip.name, Styles.ListLabel);
            }
            else
            {
                GUI.color = Colors.Invalid;
                GUILayout.Label("[NULL]", Styles.ListLabel);
                GUI.color = oldGuiColor;
            }


            if (GUI.enabled)
            {

                if (GUIEvents.MouseDown && GUIEvents.IsMouseInside(new Rect(rect.x, rect.y, rect.width - 24, rect.height)))
                {
                    result = true;
                    controller.SelectedNote.layoutIndex = controller.GetLayoutIndex(item);
                    Event.current.Use();
                }

                if (item.Clip != null && !player.IsPlaying)
                {
                    if (GhostIconButton.Draw(previewIcon))
                    {
                        PlayPreview(item);
                    }
                }
            }
            GUILayout.EndHorizontal();
            return result;
        }



        private void OnNoteSelect(Rhythm rhythm, int noteIndex)
        {
            if (audioListView != null && audioListView.List != null && audioListView.List.Count > 0)
            {
                var list = audioListView.List;
                Note note = rhythm.Notes[noteIndex];
                if (note != null)
                {
                    AudioReference audioRef = controller.GetAudioReference(note.layoutIndex);
                    if (audioRef != null)
                    {
                        audioListView.Select(list.IndexOf(audioRef));
                    }
                }
            }
        }

        private void OnSearchFieldInput(string search)
        {
            string searchWord = search.Trim();

            bool currentlySelected = false;
            AudioReference current = controller.GetCurrentAudioReference();
            List<AudioReference> filtered;

            if (searchWord.Length > 0)
            {
                filtered = controller.AudioReferences.FindAll((audioRef) => {
                    if (audioRef != null && audioRef.Clip != null)
                    {
                        if (audioRef.Clip.name.Contains(searchWord))
                        {
                            if (!currentlySelected && current != null)
                            {
                                currentlySelected = audioRef == current;
                            }
                            return true;
                        }
                    }
                    return false;
                });
            }
            else
            {
                filtered = controller.AudioReferences;
                currentlySelected = current != null;
            }

            audioListView.List = filtered;
            if (current != null && currentlySelected)
            {
                audioListView.Select(filtered.IndexOf(current));
            }
            else
            {
                audioListView.UnSelect();

            }
            Repainter.RepaintFocused();
        }


        private void PlayPreview(AudioReference item)
        {
            if (item != null && item.Clip != null)
            {
                EditorAudioUtility.StopAllClips();
                EditorAudioUtility.PlayClip(item.Clip);
            }
        }



    }
}
