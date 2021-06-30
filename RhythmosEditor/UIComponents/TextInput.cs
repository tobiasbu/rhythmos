
using UnityEditor;
using UnityEngine;

namespace RhythmosEditor.UIComponents
{
    internal class TextInput
    {

        string validValue = "";
        string currentValue = "";
        bool isEdited = false;

        public string Text
        {
            get { return validValue; }
            set
            {
                validValue = currentValue = value;
            }
        }

        public string Label { set; get; }

        public bool Draw()
        {
            //if (style == null) // Has to be done with OnGUI call
            //{
            //    style = new GUIStyle(GUI.skin.textField);
            //    style.alignment = TextAnchor.MiddleCenter;
            //}

            bool submitted = false;
            Event e = Event.current;
            if (EditorGUIUtility.editingTextField && GUI.GetNameOfFocusedControl() == Label)
            {
                Event ev = Event.current;
                if (ev.isKey)
                {
                    if (ev.keyCode == KeyCode.Return || ev.keyCode == KeyCode.KeypadEnter) {
                        Text = currentValue;
                        submitted = true;
                    } 
                    else if (e.keyCode == KeyCode.Escape)
                    {
                        Text = validValue;
                    }
                }
            }

            if (Label == null) 
            {
                currentValue = EditorGUILayout.TextField(currentValue);
            } 
            else
            {
                currentValue = EditorGUILayout.TextField(Label, currentValue);
            }
            isEdited = validValue != currentValue;

            return submitted;
        }

    }
}
