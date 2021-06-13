using UnityEngine;

namespace RhythmosEditor
{
    internal class ColorUtility {
        public static Color Change(Color color, float saturation, float value)
        {
            float h = 0, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            return  Color.HSVToRGB(h, saturation, value);

        }
    }
}

