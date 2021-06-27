using UnityEngine;
using System.Globalization;

namespace RhythmosEditor.Utils
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

        public static string Time(float timeInSeconds)
        {
            if (timeInSeconds <= 0f)
            {
                return "00:00:00";
            }

            float minutes = Mathf.Floor(timeInSeconds / 60);
            float seconds = Mathf.FloorToInt(timeInSeconds % 60);
            float mili = timeInSeconds * 100;
            mili = Mathf.RoundToInt(mili % 99);

            string m, s, mm;

            if (minutes < 10)
            {
                m = "0" + minutes.ToString();
            }
            else
            {
                m = minutes.ToString("00");
            }

            if (seconds < 10)
            {
                s = "0" + seconds.ToString();
            }
            else
            {
                s = seconds.ToString("00");
            }

            if (mili < 10)
            {
                mm = "0" + mili.ToString();
            }
            else
            {
                mm = mili.ToString();
            }

            return m + ":" + s + ":" + mm;
        }

        public static bool IsValid(string str)
        {
            return !string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str);
        }

        internal static string TimeSecMilli(float timeInSeconds)
        {
            if (timeInSeconds <= 0f)
            {
                return "0:00";
            }

            float seconds = Mathf.Floor(timeInSeconds % 60);
            int mili = Mathf.RoundToInt(timeInSeconds * 100) % 100;

            string s = (seconds < 10) ? seconds.ToString() : seconds.ToString("00");
            return s + ":" + mili.ToString("00");
        }
    }
}
