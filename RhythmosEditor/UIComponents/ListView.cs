using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RhythmosEditor.UI;

namespace RhythmosEditor.UIComponents
{
    internal class ListView<T>
    {

        // GUI variables
        private float barValue;
        private Rect currentBoxRect;

        private IList<T> viewList;
        public IList<T> List {
            set {
                if (viewList != value)
                {
                    viewList = value;
                    BarPosition = 0;
                    UnSelect();
                }
            }
            get {
                return viewList;
            }
        }


        public float BarPosition {
            private set {
                if (viewList != null)
                {
                    barValue = value;
                    if (barValue > viewList.Count)
                    {
                        barValue = viewList.Count;
                    }
                    else if (barValue < 0)
                    {
                        barValue = 0;
                    }
                }
            }
            get {
                return barValue;
            }
        }

        public T Current { private set; get; }

        public T Last { private set; get; }

        public int SelectedIndex { private set; get; } = -1;

        public float ItemHeight { set; get; } = 20;

        public bool HasSelection {
            get {
                return List != null && List.Count != 0 && SelectedIndex >= 0;
            }
        }

        public bool Focus { get; set; }
        public bool KeyboardNavigation { get; internal set; } = true;

        public Func<T, string> onGetItemLabel;
        public Action<T, int> onSelectionChange;
        internal Func<T, Rect, int, bool> onDrawItem;

        private void OnUpdate()
        {
            if (Event.current != null)
            {
                Event current = Event.current;
                Rect test = currentBoxRect;
                test.y = 0;

                if (test.Contains(current.mousePosition))
                {
                    float newBarPosition = BarPosition + GUIEvents.GetScrollDelta(current);
                    if (newBarPosition != BarPosition)
                    {
                        float max = 0;
                        float totalSize = ItemHeight * (List != null ? List.Count : 0);
                        if (totalSize > currentBoxRect.height)
                        {
                            max = (totalSize - currentBoxRect.height) / ItemHeight;
                        }
                        BarPosition = Mathf.Clamp(newBarPosition, 0, max);
                        Repainter.Request();
                        current.Use();
                        return;
                    }
                }

                if (Focus)
                {
                    if (!currentBoxRect.Contains(current.mousePosition) && current.type == EventType.Ignore)
                    {
                        Focus = false;
                        return;
                    }

                    if (KeyboardNavigation && HasSelection && current.type == EventType.KeyDown)
                    {
                        if (current.keyCode == KeyCode.DownArrow)
                        {
                            MoveSelectionCursor(1);
                            current.Use();

                        }
                        else if (current.keyCode == KeyCode.UpArrow)
                        {
                            MoveSelectionCursor(-1);
                            current.Use();
                        }

                    }
                }
            }
        }

        public void Select(int itemIndex, bool changeScroll = true)
        {
            if (List == null)
            {
                return;
            }

            if (List.Count == 0)
            {
                return;
            }

            itemIndex = Mathf.Clamp(itemIndex, 0, List.Count - 1);

            if (changeScroll)
            {
                BarPosition = itemIndex;
            }

            if (itemIndex != SelectedIndex)
            {
                Last = Current;
                Current = List[itemIndex];
                SelectedIndex = itemIndex;
                onSelectionChange?.Invoke(Current, itemIndex);
            }
            Repainter.Request();

        }

        public void UnSelect()
        {
            Last = Current;
            Current = default;
            SelectedIndex = -1;
            Repainter.Request();
        }

        public void MoveSelectionCursor(int delta)
        {

            if (List == null)
            {
                return;
            }

            if (List.Count == 0)
            {
                return;
            }

            if (!HasSelection)
            {
                Select(0);
                return;
            }

            int currentIndex = SelectedIndex;
            int nextIndex = currentIndex + delta;
            if (nextIndex == currentIndex)
            {
                return;
            }

            nextIndex = Mathf.Clamp(nextIndex, 0, List.Count - 1);
            Select(nextIndex, false);

            float itemY = nextIndex * ItemHeight;
            float itemDelta = delta >= 1 ? ItemHeight : 0;
            float barY = BarPosition * ItemHeight;
            float futurePos = itemY + itemDelta;
            Rect visibleRect = new Rect(0, barY, currentBoxRect.width, currentBoxRect.height);
            if (!visibleRect.Contains(new Vector2(0, itemY + itemDelta)))
            {

                if (futurePos > currentBoxRect.height)
                {
                    if (delta > 0)
                    {
                        BarPosition = (futurePos - currentBoxRect.height) / ItemHeight;

                    }
                    else
                    {
                        BarPosition = futurePos / ItemHeight;
                    }

                }
                else
                {
                    BarPosition = futurePos / ItemHeight;
                }
            }
        }

        private void OnDraw(Rect boxRect)
        {
            string itemLabel = "";
            int count = List != null ? List.Count : 0;
            float totalsize = (ItemHeight * count);
            float y = -(ItemHeight * barValue);
            bool select;
            Rect entryRect = new Rect(0, y, boxRect.width, ItemHeight);
            if (totalsize >= boxRect.height)
            {
                entryRect.width -= 15;
            }

            Color oldGuiColor = GUI.color;

            GUI.BeginGroup(boxRect);
            for (int i = 0; i < count; i += 1)
            {
                select = false;
                entryRect.y = y;


                if (SelectedIndex == i)
                {
                    GUI.color = Colors.Selection;
                    GUI.DrawTexture(entryRect, Icons.Pixel);
                    GUI.color = oldGuiColor;
                }
                else
                {
                    GUI.color = EditorStyles.label.normal.textColor;
                }

                if (onDrawItem != null)
                {

                    GUILayout.BeginArea(entryRect);
                    select |= onDrawItem(List[i], new Rect(0, 0, entryRect.width, entryRect.height), i);
                    GUILayout.EndArea();

                }
                else
                {

                    itemLabel = onGetItemLabel?.Invoke(List[i]);
                    GUI.Label(entryRect, itemLabel, Styles.ListLabel);
                    GUI.color = Color.clear;
                    select |= GUI.Button(entryRect, "");
                }

                if (select)
                {
                    if (SelectedIndex != i)
                    {
                        Select(i, false);
                        Focus = true;
                    }
                }

                y += ItemHeight;
            }

            GUI.color = Color.white;
            if (totalsize >= boxRect.height)
            {
                float viewableRatio = boxRect.height / totalsize;
                barValue = GUI.VerticalScrollbar(new Rect(boxRect.width - 14, 0, 30, boxRect.height), barValue, count * viewableRatio, 0, count);
            }
            else
            {
                barValue = 0;
            }

            OnUpdate();

            GUI.EndGroup();
        }

        public void Draw()
        {
            GUILayout.Box("", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));

            // Unity issue:
            // See: https://issuetracker.unity3d.com/issues/guilayoututility-dot-getlastrect-returns-incorrect-rect-when-used-after-editorguilayout-dot-dropdownbutton
            // Will not be fixed...
            if (Event.current.type == EventType.Repaint)
            {
                currentBoxRect = GUILayoutUtility.GetLastRect();
            }

            OnDraw(currentBoxRect);
            // currentBoxRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            // currentBoxRect = GUILayoutUtility.GetRect(100, 100, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            // GUI.Box(currentBoxRect, "");
        }

        public void Draw(float width, float height)
        {
            GUILayout.Box("", GUILayout.Width(width), GUILayout.Height(height));

            if (Event.current.type == EventType.Repaint)
            {
                currentBoxRect = GUILayoutUtility.GetLastRect();
            }

            OnDraw(currentBoxRect);
        }

    }
}
