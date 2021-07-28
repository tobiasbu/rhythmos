using System;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using RhythmosEditor.Utils;

namespace RhythmosEditor.UIComponents
{
    [Serializable]
    internal class SearchFieldComponent
    {
        private DebounceDispatcher debounceDispatcher;
        private SearchField searchField;

        public Action<string> onInputChanged;
        public string searchString = "";
        private int interval = 100;

        public void OnToolbarGUI()
        {
            if (searchField == null)
            {
                searchField = new SearchField();

            }

            GUILayout.Space(4);
            string result = searchField.OnToolbarGUI(searchString, GUILayout.ExpandWidth(true));

            if (result != searchString)
            {
                searchString = result;

                if (onInputChanged != null)
                {

                    if (debounceDispatcher == null)
                    {
                        debounceDispatcher = new DebounceDispatcher();
                    }

                    debounceDispatcher.Debounce(() => {
                        onInputChanged(searchString);
                    }, interval);

                }

            }
        }


        bool HasSearchbarFocused()
        {
            return GUIUtility.keyboardControl == searchField.searchFieldControlID;
        }

        internal void Clear()
        {
            searchString = "";
        }
    }
}
