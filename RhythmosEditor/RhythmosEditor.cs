using UnityEngine;
using UnityEditor;
using RhythmosEditor.Commands;
using System;

namespace RhythmosEditor
{
    internal class RhythmosEditor : EditorWindow
    {
        [SerializeField]
        private Config config;

        [SerializeField]
        private int currentToolbarSelection = 0;
        private Rect currentPageRect;

        private BaseEditorPage[] pages;
        private BaseEditorPage currentPage;
        private readonly string[] toolbar = { "Rhythms", "Audio clips", "Settings" };


        [MenuItem("Tools/Rhythmos Editor")]
        public static void Launch()
        {
            GetWindow(typeof(RhythmosEditor)).Show();
            GetWindow(typeof(RhythmosEditor)).minSize = new Vector2(400, 240);
        }

        private bool CreatePages()
        {
            bool isNull = pages == null;

            if (isNull)
            {
                pages = new BaseEditorPage[3] { new Pages.RhythmsPage(), new Pages.AudioClipsPage(), new Pages.SettingsPage() };
            }

            return isNull;
        }

        private void Load()
        {
            if (config == null)
            {
                config = new Config();
                config.Load();
                config.LoadDatabaseXML(null);

                if (CreatePages())
                {
                    currentPage = pages[0];
                }

            }
            else
            {
                CreatePages();
                currentPage = pages[currentToolbarSelection];
            }

            if (currentPage != null)
            {
                currentPage.OnLoad();
                currentPage.OnPageSelect(config);
            }
            
        }

        private void OnEnable()
        {
            UndoRedo.SetWindow(this);
            Load();
        }

        private void OnDisable()
        {
            UndoRedo.Clear();
        }

        private void OnDestroy()
        {
            UndoRedo.Clear();
            config.Save();
            config.SaveDatabaseXML(null);
        }

        private void OnGUI()
        {
            // Load stuff
            Textures.Load();

            // Main are with safe borders
            Rect areaRect = new Rect(4, 4, position.width - 8, position.height - 8);
            GUILayout.BeginArea(areaRect);

            #region Toolbar and Undo&Redo buttons
            GUILayout.BeginHorizontal();
            GUIDraw.UndoRedoButtons(false, false);

            EditorGUI.BeginChangeCheck();
            int toolbarSelection = GUILayout.Toolbar(currentToolbarSelection, toolbar);
            if (EditorGUI.EndChangeCheck())
            {
                SetPage(toolbarSelection);
            }
            GUILayout.EndHorizontal();
            #endregion


            #region Current page
            Rect pageRect;
            if (Event.current.type == EventType.Repaint)
            {
                Rect lastRect = GUILayoutUtility.GetLastRect();
                currentPageRect = new Rect(0, areaRect.y + lastRect.y + lastRect.height, areaRect.width, areaRect.height - (lastRect.y + lastRect.height));
                pageRect = currentPageRect;
            }
            else
            {
                pageRect = currentPageRect;
            }

            // Draw current page
            GUILayout.BeginArea(pageRect);
            if (currentPage != null)
            {
                bool isDisabled = currentToolbarSelection != 2 && !config.loaded;
                using (new EditorGUI.DisabledScope(isDisabled))
                {
                    currentPage.OnDraw(pageRect);
                }


            }
            GUILayout.EndArea();

            #endregion

            // Warning box
            if (!config.loaded && currentToolbarSelection < 2)
            {
                GUIDraw.WarningBox(pageRect);
            }

            GUILayout.EndArea();

            if (Repainter.ShouldRepaint)
            {
                Repaint();
                Repainter.Clear();
            }
        }

        internal void SetPage(int pageIndex)
        {
            currentToolbarSelection = pageIndex;
            currentPage = null;
            if (currentToolbarSelection < pages.Length)
            {
                EditorGUIUtility.editingTextField = false;
                GUI.FocusControl(null);

                currentPage = pages[currentToolbarSelection];
                if (currentPage != null)
                {
                    currentPage.OnLoad();
                    currentPage.OnPageSelect(config);
                }

            }
        }

    }
}

