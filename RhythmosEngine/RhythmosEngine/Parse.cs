using System.Globalization;
using UnityEngine;

namespace RhythmosEngine
{
    internal static class Parse
    { 

        public static float Float(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        public static Color Color(string col)
        {
            Color output = new Color(0, 0, 0, 0);
            if (col.StartsWith("RGBA("))
            {
                col = col.Remove(0, 5);
                col = col.Remove(col.Length - 1, 1);
            }
            string[] strs = col.Split(","[0]);

            for (int i = 0; i < strs.Length; i++)
            {
                float value = Float(strs[i]);
                output[i] = Mathf.Clamp(value, 0.0f, 1.0f);
            }
            return output;
        }
    }
}
