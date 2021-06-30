using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RhythmosEditor.UI;

namespace RhythmosEditor.UIComponents
{
    internal class ListView<T>
    {
        // GUI items
        private GUIStyle listItemStyle;

        // GUI variables
        private float barValue;
        private Rect currentBoxRect;
        private Rect layoutRect;


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
            get
            {
                return viewList;
            }
        }

        public float BarPosition
        {
            private set
            {
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
            get
            {
                return barValue;
            }
        }

        public T Current { private set; get; }

        public T Last { private set; get; }

        public int SelectedIndex { private set; get; } = -1;

        public float ItemHeight { set; get; } = 19;

        public bool HasSelection
        {
            get
            {
                return List != null && List.Count != 0 && SelectedIndex >= 0;
            }
        }


        public Func<T, string> onGetItemLabel;
        public Action<T, int> onSelectionChange;

        private void Load()
        {
            if (listItemStyle == null)
            {
                listItemStyle = new GUIStyle("label");
                GUIStyleState normalStyle = EditorStyles.label.normal;
                listItemStyle.normal = normalStyle;
                listItemStyle.normal.textColor = Color.white;
                listItemStyle.contentOffset = new Vector2(2, -1);
            }
        }

        private void OnUpdate()
        {
            if (Event.current != null)
            {
                Event current = Event.current;

                if (currentBoxRect.Contains(current.mousePosition))
                {
                    float newBarPosition = BarPosition + GUIEvents.GetScrollDelta(current);
                    if (newBarPosition != BarPosition)
                    {
                        BarPosition = newBarPosition;
                        Repainter.Request();
                        return;
                    }
                }

                if (HasSelection && current.type == EventType.KeyDown)
                {
                    if (current.keyCode == KeyCode.DownArrow)
                    {
                        MoveSelectionCursor(1);

                    }
                    else if (current.keyCode == KeyCode.UpArrow)
                    {
                        MoveSelectionCursor(-1);
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
                Repainter.Request();
            }

            if (itemIndex != SelectedIndex)
            {
                Last = Current;
                Current = List[itemIndex];
                SelectedIndex = itemIndex;
                onSelectionChange?.Invoke(Current, itemIndex);
                Repainter.Request();
            }
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

            int index = SelectedIndex + delta;
            if (index == SelectedIndex)
            {
                return;
            }

            index = Mathf.Clamp(index, 0, List.Count - 1);
            Select(index, false);

            float itemY = index * ItemHeight;
            float itemDelta = delta >= 1 ? ItemHeight : 0;
            float barY = BarPosition * ItemHeight;
            Rect visibleRect = new Rect(0, barY, currentBoxRect.width, currentBoxRect.height);
            if (!visibleRect.Contains(new Vector2(0, itemY + itemDelta)))
            {
                if (itemY + itemDelta > currentBoxRect.height)
                {
                    BarPosition = (itemY + itemDelta - currentBoxRect.height) / ItemHeight;
                }
                else
                {
                    BarPosition = index;
                }
            }

        }

        private void OnDraw(Rect boxRect) {

            Load();

            string itemLabel = "";
            int count = List != null ? List.Count : 0;
            float totalsize = (ItemHeight * count);
            float y = -(ItemHeight * barValue);

            Rect groupRect = boxRect;
            Rect entryRect;

            groupRect.width = boxRect.x + boxRect.width;
            groupRect.x = 0;
            entryRect = new Rect(boxRect.x, y, boxRect.width, ItemHeight);
            if (totalsize > boxRect.height)
            {
                entryRect.width -= 14;
            }

            GUI.BeginGroup(groupRect);
            for (int i = 0; i < count; i += 1)
            {
                entryRect.y = y;
                if (onGetItemLabel != null)
                {
                    itemLabel = onGetItemLabel(List[i]);
                }

                if (SelectedIndex == i)
                {
                    GUI.color = Colors.Selection;
                    GUI.DrawTexture(entryRect, Icons.Pixel);
                    GUI.color = Color.white;
                }
                else
                {
                    GUI.color = EditorStyles.label.normal.textColor;
                }

                GUI.Label(entryRect, itemLabel, listItemStyle);
                GUI.color = Color.clear;
                if (GUI.Button(entryRect, ""))
                {
                    if (SelectedIndex != i)
                    {
                        Select(i, false);
                    }
                }
                y += ItemHeight;
            }

            GUI.color = Color.white;
            if (totalsize >= boxRect.height)
            {
                float viewableRatio = boxRect.height / totalsize;
                barValue = GUI.VerticalScrollbar(new Rect(groupRect.width - 14, 0, 30, groupRect.height), barValue, count * viewableRatio, 0, count);
            }
            else
            {
                barValue = 0;
            }

            OnUpdate();

            GUI.EndGroup();
        }

        public void Draw() {
            layoutRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            GUI.Box(layoutRect, "");
            OnDraw(layoutRect);
        }

        public void Draw(float width, float height)
        {

            GUILayout.Box("", GUILayout.Width(width), GUILayout.Height(height));


            // Unity issue:
            // See: https://issuetracker.unity3d.com/issues/guilayoututility-dot-getlastrect-returns-incorrect-rect-when-used-after-editorguilayout-dot-dropdownbutton
            // Will not be fixed...
            if (Event.current.type == EventType.Repaint)
            {
                currentBoxRect = GUILayoutUtility.GetLastRect();
            }

            OnDraw(currentBoxRect);

            //Load();

            //string itemLabel = "";
            //int count = List != null ? List.Count : 0;
            //float totalsize = (ItemHeight * count);
            //float y = -(ItemHeight * barValue);

           

            //Rect boxRect;
            //Rect groupRect;
            //Rect entryRect;

            //// Unity issue:
            //// See: https://issuetracker.unity3d.com/issues/guilayoututility-dot-getlastrect-returns-incorrect-rect-when-used-after-editorguilayout-dot-dropdownbutton
            //// Will not be fixed...
            //if (Event.current.type == EventType.Repaint)
            //{
            //    boxRect = GUILayoutUtility.GetLastRect();
            //    currentBoxRect = boxRect;
            //}
            //else
            //{
            //    boxRect = currentBoxRect;
            //}

            //groupRect = boxRect;
            //groupRect.width = boxRect.x + width;
            //groupRect.x = 0;
            //entryRect = new Rect(boxRect.x, y, width, ItemHeight);
            //if (totalsize > boxRect.height)
            //{
            //    entryRect.width -= 14;
            //}

            //GUI.BeginGroup(groupRect);
            //for (int i = 0; i < count; i += 1)
            //{
            //    entryRect.y = y;
            //    if (onGetItemLabel != null)
            //    {
            //        itemLabel = onGetItemLabel(List[i]);
            //    }

            //    if (SelectedIndex == i)
            //    {
            //        GUI.color = Colors.Selection;
            //        GUI.DrawTexture(entryRect, Icons.Pixel);
            //        GUI.color = Color.white;
            //    }
            //    else
            //    {
            //        GUI.color = EditorStyles.label.normal.textColor;
            //    }

            //    GUI.Label(entryRect, itemLabel, listItemStyle);
            //    GUI.color = Color.clear;
            //    if (GUI.Button(entryRect, ""))
            //    {
            //        if (SelectedIndex != i)
            //        {
            //            Select(i, false);
            //        }
            //    }
            //    y += ItemHeight;
            //}

            //GUI.color = Color.white;
            //if (totalsize >= boxRect.height)
            //{
            //    float viewableRatio = boxRect.height / totalsize;
            //    barValue = GUI.VerticalScrollbar(new Rect(groupRect.width - 14, 0, 30, groupRect.height), barValue, count * viewableRatio, 0, count);
            //}
            //else
            //{
            //    barValue = 0;
            //}

            //OnUpdate();

            //GUI.EndGroup();
        }

    }
}
