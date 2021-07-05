using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RhythmosEditor.Settings;

namespace RhythmosEditor.Pages
{
    [Serializable]
    internal sealed class PageManager : IPageManager
    {
        [SerializeField]
        private int pageIndex;

        private List<IEditorPage> pagesList;
        private string[] pageNames;

        private Rect pageRect;
        private IEditorPage current;

        private RhythmosConfig config;
        private EditorWindow editorWindow;

        public int Selection
        {
            get {
                return pageIndex;
            }
        }

        public int Count {
            get {
                return pagesList != null ? pagesList.Count : 0;
            }
        }

        public void Init(EditorWindow editorWindow, RhythmosConfig config)
        {
            this.editorWindow = editorWindow;
            this.config = config;
        }

        public void Draw(Rect window)
        {
            // Main area with safe borders
            Rect areaRect = new Rect(4f, 4f, Mathf.Ceil(window.width - 8), 24f);

            #region Toolbar and Undo&Redo buttons

            GUILayout.Space(areaRect.y);

            GUILayout.BeginHorizontal(GUILayout.Height(24), GUILayout.Width(window.width - 4));
            GUILayout.Space(areaRect.x);

            Components.UndoRedoButtons(false, false);

            EditorGUI.BeginChangeCheck();
            int toolbarSelection = GUILayout.Toolbar(Selection, pageNames, GUILayout.Height(20));
            if (EditorGUI.EndChangeCheck())
            {
                SetPage(toolbarSelection);
            }
            GUILayout.EndHorizontal();

            #endregion

            #region Current page

            float y = Mathf.Ceil(areaRect.y + areaRect.height + 2);
            pageRect = new Rect(areaRect.x,
                    y,
                    areaRect.width,
                    window.height - y
                    );

            GUILayout.BeginArea(pageRect);

            if (current != null)
            {
                bool isDisabled = pageIndex != 2 && !config.loaded;
                using (new EditorGUI.DisabledScope(isDisabled))
                {
                    current.OnDraw(pageRect);
                }
            }


            GUILayout.EndArea();

            #endregion
        }

        public void AddPage(string pageName, IEditorPage editorPage)
        {
            if (pagesList == null)
            {
                pagesList = new List<IEditorPage>();
                pageNames = new string[0];
            }

            pagesList.Add(editorPage);

            int count = pagesList.Count;
            if (count > pageNames.Length)
            {
                Array.Resize(ref pageNames, count);
            }
            pageNames[count - 1] = pageName;

            if (pagesList.Count == 1 && current == null)
            {
                pageIndex = 0;
                current = pagesList[0];
            }
        }

        public void SetPage(int pageIndex, bool force = false)
        {
            if (!force && pageIndex == this.pageIndex)
            {
                return;
            }

            if (pageIndex >= 0 && pageIndex < pagesList.Count)
            {
                current = null;
                this.pageIndex = pageIndex;

                EditorGUIUtility.editingTextField = false;
                GUI.FocusControl(null);

                current = pagesList[this.pageIndex];
                if (current != null)
                {
                    current.OnLoad();
                    current.OnPageSelect(config);
                }
            }
        }

        public void SetDirty()
        {
            if (editorWindow != null)
            {
                EditorUtility.SetDirty(editorWindow);
            }
        }
    }
}
