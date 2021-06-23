using RhythmosEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RhythmosEditor.Pages
{
    internal class AudioClipsPage : BaseEditorPage
    {

        // GUI itens
        private GUIContent addContentButton;
        private GUIContent deleteContentButton;

        // GUI variables
        private float[] tableTitleSizes = { 0.05f, 0.15f };
        private float itemHeight = 24;
        private float itemInitialSpacing = 4;
        private float barValue = 0;
        private Rect boxRect;
        private bool dragging = false;

        // References
        private List<AudioReference> audioReferences;
        private Tuple<AudioReference, int> removalTuple;
        private GUIStyle centeredLabel;

        public AudioClipsPage()
        { }

        public override void OnLoad()
        {
            if (addContentButton == null)
            {
                addContentButton = new GUIContent(Textures.Add, "Add AudioClip reference");
            }

            if (deleteContentButton == null)
            {
                deleteContentButton = new GUIContent(Textures.Delete, "Remove AudioClip reference");
            }

        }

        public override void OnPageSelect(Config config)
        {
            if (config.loaded)
            {
                audioReferences = config.RhythmosDatabase.AudioReferences;
                removalTuple = null;
            }
        }

        public override void OnDraw(Rect pageRect)
        {

            if (centeredLabel == null)
            {
                centeredLabel = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            }

            int count = audioReferences != null ? audioReferences.Count : 0;

            boxRect = new Rect(pageRect.x, itemHeight, pageRect.width, pageRect.height - (itemHeight * 2 + 10));
            Rect tableTitleRect = new Rect(0, 0, pageRect.width - 18 - 2, itemHeight);
            Rect entryRect = new Rect(2, itemInitialSpacing, boxRect.width - 2 - 18, itemHeight);

            #region Tables titles
            GUILayout.BeginArea(tableTitleRect, "");
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID", GUILayout.MaxWidth(entryRect.width * tableTitleSizes[0]), GUILayout.ExpandHeight(true));
            GUILayout.Label("Color", GUILayout.Width(entryRect.width * tableTitleSizes[1]), GUILayout.ExpandHeight(true));
            GUILayout.Label("Audio Clip", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // GUILayout.Label("Name", GUILayout.MaxWidth(tableTitlesWidthWithoutRemove * 0.395f), GUILayout.ExpandHeight(true));
            GUILayout.Label("", GUILayout.Width(28));
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            #endregion

            GUI.Box(boxRect, "");
            GUI.BeginGroup(boxRect);

            #region Table content

            float totalsize = 0;
            entryRect.y += -itemHeight * barValue;
            for (int i = 0; i < count; i += 1)
            {
                DrawItem(entryRect, audioReferences[i], i);
                totalsize += itemHeight;
                entryRect.y += itemHeight;
            }

            #endregion

            RemoveItem();

            if (dragging)
            {
                Rect dragRect = boxRect;
                dragRect.y = 0;
                GUI.Box(dragRect, "");
                GUI.Label(dragRect, "Drop your AudioClips here", centeredLabel);
            }

            #region Table scroll bar

            if (totalsize >= boxRect.height)
            {
                float viewableRatio = boxRect.height / totalsize;
                barValue = GUI.VerticalScrollbar(new Rect(boxRect.width - 14, 0, 14, boxRect.height), barValue, count * viewableRatio, 0, count);
            }
            else
            {
                GUI.enabled = false;
                barValue = 0;
                GUI.VerticalScrollbar(new Rect(boxRect.width - 14, 0, 30, boxRect.height), barValue, count, 0, count);
            }
            GUI.enabled = true;

            #endregion

            GUI.EndGroup();

            #region Bottom buttons

            GUILayout.Space(boxRect.height + itemHeight + 6);

            GUI.enabled = audioReferences != null;

            GUILayout.BeginHorizontal();

            if (GUIDraw.IconButton(addContentButton))
            {
                AddItem();
                //NoteLayout noteLayout = new NoteLayout("New Note " + (count + 1));
                //noteLayout.Color = UnityEngine.Random.ColorHSV(0f, 1f, 0.3f, 0.5f, 0.75f, 1);
                //noteList.Add(noteLayout);

                ////_undoManager.RecordNote(LayoutList[LayoutList.Count - 1], "New Note Layout", LayoutList.Count - 1, true);
                //float totalSizeWithNewItem = totalsize + itemHeight;
                //if (totalSizeWithNewItem >= boxRect.height)
                //{
                //    barValue = (totalSizeWithNewItem - boxRect.height) / itemHeight;
                //}
            }
            GUILayout.EndHorizontal();

            #endregion

            OnUpdate(totalsize);
        }

        private void DrawItem(Rect entryRect, AudioReference item, int index)
        {
            GUILayout.BeginArea(entryRect, "");
            GUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));

            EditorGUILayout.LabelField(index.ToString() + ".", GUILayout.MaxWidth(entryRect.width * tableTitleSizes[0]));

            EditorGUI.BeginChangeCheck();
            Color color = EditorGUILayout.ColorField(item.Color, GUILayout.Width(entryRect.width * tableTitleSizes[1]));
            if (EditorGUI.EndChangeCheck())
            {
                //_undoManager.RecordNote(_s, "Set Color", i, true);
                item.Color = color;
                //ReinsertEntry(_s, i);
            }

            EditorGUI.BeginChangeCheck();
            AudioClip clip = (AudioClip)EditorGUILayout.ObjectField(item.Clip, typeof(AudioClip), false, GUILayout.ExpandWidth(true));
            if (EditorGUI.EndChangeCheck())
            {
                //_undoManager.RecordNote(_s, "Set Clip", i, true);
                item.Clip = clip;
                //ReinsertEntry(_s, i);
            }



            if (GUIDraw.IconButton(deleteContentButton, GUILayout.Width(28), GUILayout.Height(19)))
            {
                //_undoManager.RecordNote(_s, "Remove Note Layout", i, true);
                removalTuple = new Tuple<AudioReference, int>(item, index);
                //EditorGUIUtility.ExitGUI();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void OnUpdate(float totalSize)
        {
            if (Event.current != null)
            {
                Event current = Event.current;
                if (boxRect.Contains(current.mousePosition))
                {
                    // Scroll
                    if (current.type == EventType.ScrollWheel)
                    {
                        int delta = GUIEvents.GetScrollDelta(current);
                        float newBarValue = barValue + delta;
                        if (newBarValue != barValue)
                        {
                            if (totalSize > boxRect.height)
                            {
                                float max = (totalSize - boxRect.height) / itemHeight;
                                newBarValue = Mathf.Clamp(newBarValue, 0, max);
                                barValue = newBarValue;
                                Repainter.Request();
                            }
                        }

                        return;
                    }

                    // Drag&Drop
                    AudioClip[] result = DragDrop<AudioClip>.Do(current);
                    if (DragDrop<AudioClip>.Valid)
                    {
                        if ((int)current.type >= 9 && (int)current.type <= 10)
                        {

                            Rect invalidRect = boxRect;
                            invalidRect.x = boxRect.x + boxRect.width * 0.2f;
                            invalidRect.width = boxRect.width - invalidRect.x - 48;
                            invalidRect.height = itemHeight * (audioReferences != null ? audioReferences.Count : 0);
                            if (invalidRect.Contains(current.mousePosition))
                            {
                                dragging = false;
                                return;
                            }
                            dragging = true;
                        }
                    }
                    else
                    {
                        dragging = false;
                    }

                    if (result != null && dragging)
                    {
                        AddRange(result);
                        dragging = false;
                    }

                    Repainter.Request();
                }
                else
                {
                    if (dragging)
                    {
                        dragging = false;
                        Repainter.Request();
                    }
                }

            }
           
        }

        private void AddItem(AudioClip clip = null)
        {
            int count = audioReferences != null ? audioReferences.Count : 0;
            AudioReference audioReference = new AudioReference();
            audioReference.Clip = clip;
            audioReference.Color = UnityEngine.Random.ColorHSV(0f, 1f, 0.3f, 0.5f, 0.75f, 1);
            audioReferences.Add(audioReference);

            //_undoManager.RecordNote(LayoutList[LayoutList.Count - 1], "New Note Layout", LayoutList.Count - 1, true);
            float totalSizeWithNewItem = (itemHeight * count) + itemHeight;
            if (totalSizeWithNewItem >= boxRect.height)
            {
                barValue = (totalSizeWithNewItem - boxRect.height) / itemHeight;
            }
        }

        private void AddRange(AudioClip[] clips)
        {
            for (int i = 0; i < clips.Length; i += 1)
            {
                AddItem(clips[i]);
            }

            Repainter.Request();
        }

        private void RemoveItem()
        {
            if (removalTuple != null)
            {
                audioReferences.Remove(removalTuple.Item1);
                int index = removalTuple.Item2;

                float itemY = index * itemHeight;
                if (itemY >= boxRect.height)
                {
                    barValue = (itemY - boxRect.height) / itemHeight;
                }

                removalTuple = null;

                Repainter.Request();
            }
        }


    }
}
