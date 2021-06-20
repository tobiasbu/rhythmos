using UnityEngine;
using UnityEditor;

namespace RhythmosEditor
{
    internal class RhythmosEditor : EditorWindow
    {
        [SerializeField]
        private Config config;

        [SerializeField]
        private int m_toolbarSelection = 0;

        private IEditorPage[] pages;
        private IEditorPage currentPage;

        private readonly string[] toolbar = { "Rhythms", "Audio clips", "Settings" };

        
        private Rect currentPageRect;

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
                pages = new IEditorPage[3] { new Pages.RhythmsPage(), new Pages.AudioClipsPage(), new Pages.SettingsPage() };
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
                currentPage = pages[m_toolbarSelection];
            }

            if (currentPage != null)
            {
                currentPage.OnLoad();
                currentPage.OnPageSelect(config);
            }
            
        }

        private void OnEnable()
        {
            Load();
        }

        private void OnDestroy()
        {
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
            GUIDraw.UndoRedo(null, false, false);

            EditorGUI.BeginChangeCheck();
            int toolbarSelection = GUILayout.Toolbar(m_toolbarSelection, toolbar);
            if (EditorGUI.EndChangeCheck())
            {
                m_toolbarSelection = toolbarSelection;

                currentPage = null;
                if (m_toolbarSelection < pages.Length)
                {
                    currentPage = pages[m_toolbarSelection];
                    if (currentPage != null)
                    {
                        EditorGUIUtility.editingTextField = false;
                        GUI.FocusControl(null);
                        currentPage.OnLoad();
                        currentPage.OnPageSelect(config);
                    }

                }
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
                bool isDisabled = m_toolbarSelection != 2 && !config.loaded;
                using (new EditorGUI.DisabledScope(isDisabled))
                {
                    currentPage.OnDraw(pageRect);
                }


            }
            GUILayout.EndArea();

            #endregion

            // Warning box
            if (!config.loaded && m_toolbarSelection < 2)
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

    }
}

