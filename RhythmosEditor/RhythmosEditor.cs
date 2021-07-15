using UnityEngine;
using UnityEditor;
using RhythmosEditor.Commands;
using RhythmosEditor.Settings;
using RhythmosEditor.Utils;
using RhythmosEditor.UI;
using RhythmosEditor.UIComponents;
using RhythmosEditor.Pages;

namespace RhythmosEditor
{
    internal class RhythmosEditor : EditorWindow
    {
        [SerializeField]
        private RhythmosConfig config;

        [SerializeField]
        private Pages.PageManager pageManager;
        private RhythmsPage rhythmPage;

        private Texture2D logoIcon;


        [MenuItem("Tools/Rhythmos Editor")]
        public static void Launch()
        {
            RhythmosEditor window = (RhythmosEditor)GetWindow(typeof(RhythmosEditor));
            window.Show();
            window.minSize = new Vector2(400, 240);

        }

        bool CreatePages()
        {
            bool create = pageManager == null;

            if (create)
            {
                pageManager = new Pages.PageManager();
            }

            pageManager.Init(this, config);

            if (pageManager.Count == 0)
            {

                pageManager.AddPage("Rhythm", new Pages.RhythmsPage());
                pageManager.AddPage("AudioClips", new Pages.AudioClipsPage());
                pageManager.AddPage("Settings", new Pages.SettingsPage());
                return true;
            }
            return create;
        }

        private void Load()
        {
            int startPage = 0;

            if (config == null)
            {
                config = new RhythmosConfig();
                config.Load();
                config.LoadDatabaseXML(null);
            }
            else
            {
                startPage = pageManager.Selection;
            }

            if (CreatePages())
            {
                startPage = 0;
            }
            pageManager.OnPageChange -= OnPageChange;
            pageManager.OnPageChange += OnPageChange;
            pageManager.SetPage(startPage, true);


            if (logoIcon == null)
            {
                logoIcon = TextureUtility.CreateFromBase64(
                    "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAkklEQVQ4T2NgoDVQbW79D7JDpq+PE5tdTNgE0cVAhjwpKvqOLg7iE2UASCHMJeiGEG0ALkOIMuB2bTUjus0wPlEGgBTDDEH3CtEGIBuC7BqSDMDlDZziUg0NXMhORnc+To3IEoQ0saCbgkuDanMbB+PfX8y3Ghq+IutBiR5cmimKRnyaQS7BSCDIriCkGd37ZPEBFYsuF+4vwmkAAAAASUVORK5CYII="
                );
            }

            titleContent = new GUIContent("RhythmosEditor", logoIcon);
        }

        private void OnPageChange(int index, IEditorPage page)
        {
            if (index == 0)
            {
                rhythmPage = (Pages.RhythmsPage)page;
            }
        }

        private void OnEnable()
        {
            Load();
            UndoRedo.SetPageManager(pageManager);
            wantsMouseEnterLeaveWindow = true;
        }

        private void OnDestroy()
        {
            UndoRedo.Clear();
            config.Save();
            config.SaveDatabaseXML(null);
        }

        private void Update()
        {
            if (pageManager.Selection == 0 && rhythmPage != null)
            {
                if (rhythmPage.IsRhythmPlaying)
                {
                    rhythmPage.Update();
                }
            }
        }

        private void OnGUI()
        {
            // Load stuff
            Colors.Load();
            Icons.Load();
            Styles.Load();


            GUIEvents.OnUpdate();
            GhostIconButton.OnUpdate();

            // Draw current pages
            pageManager.Draw(position);

            // Warning box
            if (!config.loaded && pageManager.Selection < 2)
            {
                Components.WarningBox(position);
            }

            if (Repainter.ShouldRepaint)
            {
                Repaint();
                Repainter.Clear();
            }
        }
    }
}
