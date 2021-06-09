using UnityEngine;

namespace RhythmosEditor
{
    internal class ColorUtility {

        public static HSV ToHSV(Color rgb)
        {
            float h = 0, s;
            float cmax = Mathf.Max(rgb.r, rgb.g, rgb.b);
            float cmin = Mathf.Min(rgb.r, rgb.g, rgb.b);
            float delta = cmax - cmin;

            if (delta > 0.0f) {

                if (rgb.r == cmax)
                {
                    h = ((rgb.g - rgb.b) / delta) % 6.0f;
                }
                else if (rgb.g == cmax)
                {
                    h = 2.0f + ((rgb.b - rgb.r) / delta);
                }
                else if (rgb.b == cmax)
                {
                    h = 4.0f + ((rgb.r - rgb.g) / delta);
                }

                if (cmax > 0)
                {
                    s = delta / cmax;
                }
                else
                {
                    s = 0.0f;
                }

            } else {
                h = 0.0f;
                s = 0.0f;
            }

            h *= 60f;
            if (h < 0.0f)
            {
                h = h + 360f;
            }

            return new HSV(h / 360.0f, s, cmax);
        }

        public static Color ParseColor(string col) {
			
			Color output = new Color(0,0,0,0);
			if (col.StartsWith("RGBA(")) {
				
				col = col.Remove(0,5);
				col = col.Remove(col.Length-1,1);
				
			} 
			string[] strs = col.Split(","[0]);
			
			for (int i = 0; i < strs.Length; i++) {
                float value = Parser.ToInvariantFloat(strs[i]);
                output[i] = Mathf.Clamp(value, 0.0f, 1.0f);
			}
			
			
			
			return output;
		}

        public static string ToInvariantString(Color color)
        {
            string str = "RGBA(";
            str = string.Concat(str, color.r.FloatToInvariant(), ", ");
            str = string.Concat(str, color.g.FloatToInvariant(), ", ");
            str = string.Concat(str, color.b.FloatToInvariant(), ", ");
            str = string.Concat(str, color.a.FloatToInvariant(), ")");
            return str;
        }

        public static Color Change(Color color, float saturation)
        {
            float h = 0, s, v;
            Color.RGBToHSV(color, out h, out s, out v);

            v += saturation;
            return Color.HSVToRGB(h, s, v, false);

        }

        public static Color Change(Color color, float saturation, float value)
        {
            float h = 0, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            return  Color.HSVToRGB(h, saturation, value);

        }
    }
}

