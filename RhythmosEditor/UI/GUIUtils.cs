using UnityEngine;

namespace RhythmosEditor.UI
{
    public static class GUIUtils
    {
        public static void SetColor(Color color)
        {
            if (GUI.enabled)
            {
                GUI.color = color;
            } 
            else
            {
                color.a *= 0.675f;
                GUI.color = color;
            }

        }
    }
}
