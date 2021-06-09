using System.Globalization;

namespace RhythmosEditor
{
    internal static class Parser
    {
        private static readonly CultureInfo cultureInfo = CultureInfo.InvariantCulture;

        public static string FloatToInvariant(this float value)
        {
            return value.ToString(cultureInfo);
        }

        public static float ToInvariantFloat(string value)
        {
            return float.Parse(value, cultureInfo);
        }

        
    }
}