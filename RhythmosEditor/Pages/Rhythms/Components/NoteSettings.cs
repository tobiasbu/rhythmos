using UnityEngine;
using UnityEditor;
using RhythmosEditor.UI;

namespace RhythmosEditor.Pages.Rhythms
{
    internal class NoteSettings
    {
        // GUI items
        private GUIStyle switchBackground;
        private GUIStyle miniLabelStyle;

        // GUI variables
        private const float switchTextWidth = 32;
        private const float switchHeight = 24;
        private const float switchIconContainerWidth = 24;
        private const float switchMinWidth = (switchHeight * 2) + switchTextWidth;

        private Color noteColor;
        private float noteColorAlphaUnselected;

        // Beat properties
        private readonly string[] beatsStrings = { "4", "2", "1", "1/2", "1/4", "1/8", "1/16", "1/32", "1/64", "1/128" };
        private readonly float[] beatsValue = { 4f, 2f, 1f, 0.5f, 0.25f, 0.125f, 0.0625f, 1f / 32f, 1f / 64f, 1f / 128f };
        private const int fractionalStartIndex = 2;

        // References
        internal EditController controller;

        public void Draw(Rect rhythmRect)
        {
            if (EditorGUIUtility.isProSkin)
            {
                noteColor = Colors.LightGray;
                noteColorAlphaUnselected = 0.45f;
            }
            else
            {
                noteColor = Colors.DarkGray;
                noteColorAlphaUnselected = 0.6f;
            }

            if (switchBackground == null)
            {
                switchBackground = new GUIStyle("ShurikenEffectBg");
            }

            if (miniLabelStyle == null)
            {
                miniLabelStyle = new GUIStyle(EditorStyles.miniLabel);
                miniLabelStyle.alignment = TextAnchor.MiddleCenter;
                miniLabelStyle.padding.left = 0;
                miniLabelStyle.padding.right = 0;
                miniLabelStyle.contentOffset = Vector2.zero;

            }

            Color oldGuiColor = GUI.color;
            GUI.enabled = controller.HasNoteSelected;
            float durationSpacing = 4;
            if (rhythmRect.width > 360)
            {
                durationSpacing = Mathf.Clamp(4 + ((rhythmRect.width - 360) * 0.2f), 4, 20);
            }

            // Note Edition Header
            GUILayout.BeginHorizontal();
            Components.MiniLabel("Type", TextAnchor.MiddleLeft, GUILayout.Width(switchMinWidth + durationSpacing));
            Components.MiniLabel("Duration");
            GUILayout.EndHorizontal();

            // Components
            GUILayout.BeginHorizontal(GUILayout.Height(52));
            GUILayout.Space(4);

            // Rest/Note switcher
            GUILayout.BeginVertical(GUILayout.Width(switchMinWidth));
            GUILayout.FlexibleSpace();
            if (NoteSwitcher(controller.CurrentNoteIsRest))
            {
                controller.SelectedNote.isRest = !controller.CurrentNoteIsRest;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();

            GUILayout.Space(durationSpacing);

            // Note duration
            GUILayout.BeginVertical();
            DrawSelectionGrid(rhythmRect.width);
            GUILayout.Space(4);
            GUILayout.EndVertical();

            GUILayout.Space(4);
            GUILayout.EndHorizontal();

            Components.HorizontalLine();
            GUI.color = oldGuiColor;

        }

        private bool NoteSwitcher(bool isRest)
        {
            bool result = false;
            Rect rect = GUILayoutUtility.GetRect((switchIconContainerWidth * 2) + switchTextWidth, switchHeight);

            // Background
            GUIUtils.SetColor(Colors.Gray(1, 0.5f));
            GUI.Box(rect, "", switchBackground);

            // Switch head
            if (GUI.enabled)
            {
                Rect headRect = new Rect(rect.x, rect.y, switchIconContainerWidth + switchTextWidth, rect.height - 1);
                headRect.x += isRest ? switchIconContainerWidth : 0;
                GUI.color = Color.white;
                GUI.Label(headRect, "", GUI.skin.button);

                headRect.x = rect.x + switchIconContainerWidth + (isRest ? 6 : -6);
                headRect.width = switchTextWidth;
                GUI.Label(headRect, isRest ? "Rest" : "Note");
            }

            if (GUI.enabled)
            {
                GUI.color = Color.clear;
                result |= GUI.Button(rect, "");

                if (Event.current != null)
                {
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        GUI.color = Colors.Gray(1, EditorGUIUtility.isProSkin ? 0.025f : 0.15f);
                        GUI.DrawTexture(rect, Icons.Pixel);
                    }
                }
            }

            // Symbols
            Rect noteRect = new Rect(rect.x + 2, rect.y + 4, 16, 16);
            if (GUI.enabled)
            {
                GUI.color = GUIUtils.GetColor(noteColor, isRest ? noteColorAlphaUnselected : 1f);
                GUI.DrawTexture(noteRect, Icons.Note);
                GUI.color = GUIUtils.GetColor(noteColor, !isRest ? noteColorAlphaUnselected : 1f);
                noteRect.x += switchIconContainerWidth + switchTextWidth + 2;
                GUI.DrawTexture(noteRect, Icons.Rest);
            }
            else
            {
                GUI.color = GUIUtils.GetColor(noteColor, noteColorAlphaUnselected);
                GUI.DrawTexture(noteRect, Icons.Note);
                noteRect.x += switchIconContainerWidth + switchTextWidth + 2;
                GUI.DrawTexture(noteRect, Icons.Rest);
            }

            return result;
        }

        private void DrawSelectionGrid(float areaWidth, int xCount = 5)
        {
            int maxRows = Mathf.FloorToInt(beatsStrings.Length / xCount);
            float dur;
            bool containsMouse = false, selected, fistRowDone = false;
            int i = 0;
            Rect rect = GUILayoutUtility.GetRect(100, 40, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (rect.width > 280)
            {
                rect.width = 280;
            }
            Rect cell = rect;
            cell.height = Mathf.Ceil(rect.height / maxRows);
            cell.width = Mathf.Ceil(rect.width / xCount);
            Rect highlighRect = rect;
            Rect halfCell = new Rect(cell.position, new Vector2(cell.width, cell.height / 2));
            Rect line = halfCell;
            line.width = halfCell.width / 2;
            line.x = line.width * 0.5f;
            line.height = 0.5f;

            if (areaWidth < 320)
            {
                miniLabelStyle.fontSize = 9;
            }
            else
            {
                miniLabelStyle.fontSize = 11;
            }

            Components.OutlineBox(rect, GUI.enabled ? Colors.EnabledHorizontalLine : Colors.DisabledHorizontalLine);

            while (i < beatsStrings.Length)
            {
                if (GUI.enabled)
                {
                    dur = beatsValue[i];
                    selected = dur > 0 && Mathf.Approximately(controller.CurrentNoteDuration, dur);

                    GUI.color = Color.clear;
                    if (GUI.Button(cell, ""))
                    {
                        controller.SelectedNote.duration = dur;
                    }

                    if (Event.current != null)
                    {
                        containsMouse = cell.Contains(Event.current.mousePosition);
                    }

                    if (selected || containsMouse)
                    {
                        highlighRect = new Rect(cell.x + 1, cell.y + 1, cell.width, cell.height - 1);
                        if (highlighRect.xMax > rect.xMax)
                        {
                            highlighRect.width -= (highlighRect.xMax - rect.xMax);
                        }
                        highlighRect.width -= 1;
                    }

                    if (containsMouse || selected)
                    {
                        if (selected)
                        {
                            GUI.color = EditorGUIUtility.isProSkin ? Colors.Selection : Colors.Gray(1, 0.65f);
                        }
                        else
                        {
                            GUI.color = EditorGUIUtility.isProSkin ? Colors.Gray(1, 0.1f) : Colors.Gray(1, 0.25f);

                        }
                        GUI.DrawTexture(highlighRect, Icons.Pixel);
                    }
                }

                Color color = Color.white;
                GUI.color = color;
                if (areaWidth < 240 && i > fractionalStartIndex)
                {
                    miniLabelStyle.fontSize = 9;
                    halfCell.position = cell.position;
                    GUI.Label(halfCell, "1", miniLabelStyle);

                    miniLabelStyle.fontSize = i > 8 ? 8 : 9;
                    halfCell.y += halfCell.height + (i > 8 ? 1 : 0);
                    GUI.Label(halfCell, beatsStrings[i].Split('/')[1], miniLabelStyle);

                    line.x = cell.x + (line.width * 0.5f);
                    line.y = cell.y + halfCell.height;
                    if (GUI.enabled)
                    {
                        color.a = EditorGUIUtility.isProSkin ? 0.6f : 1f;
                    }
                    else
                    {
                        color.a = EditorGUIUtility.isProSkin ? 0.25f : 0.3f;
                    }
                    GUI.color = color;
                    GUI.DrawTexture(line, Icons.Pixel);
                }
                else
                {
                    GUI.Label(cell, beatsStrings[i], miniLabelStyle);
                }

                cell.x += cell.width;
                if ((i + 1) % xCount == 0)
                {
                    cell.x = rect.x;
                    cell.y += cell.height;
                    Components.HorizontalLine(cell.position, rect.width);
                    fistRowDone = true;
                }
                else if (!fistRowDone)
                {
                    Components.VerticalLine(cell.position, rect.height);
                }

                i += 1;
            }
        }

        private float IndexToDuration(int index)
        {
            switch (index)
            {
                case 0:
                    return 4f;
                case 1:
                    return 2f;
                case 2:
                    return 1f;
                case 3:
                    return 0.5f;
                case 4:
                    return 0.25f;
                case 5:
                    return 1f / 8f;
                case 6:
                    return 1f / 16f;
                case 7:
                    return 1f / 32f;
                case 8:
                    return 1f / 64f;
                case 9:
                    return 1f / 128f;
            }

            return -1;

        }
    }
}
