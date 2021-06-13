using UnityEngine;
using System.Globalization;

namespace RhythmosEditor
{
    internal static class StringUtils
    {
        public static string Float(float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string Color(Color color)
        {
            return string.Concat("RGBA(",
                Float(color.r), ", ",
                Float(color.g), ", ",
                Float(color.b), ", ",
                Float(color.a),
                ")");
        }

        public static bool IsValid(string str)
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        }

    }
}
