using System;
using UnityEngine;

namespace RhythmosEditor

{
	public struct HSV {

		public float h;
		public float s;
		public float v;
		public HSV(float h, float s, float v)
		{
			this.h = h;
			this.s = s;
			this.v = v;
		}

		public override string ToString ()
		{
			return string.Format ("[HSV]" + h + ", " + s + ", " + v );
		}

	};

	internal static class Useful
	{
		public static HSV RGBToHSV(Color rgb)
		{
			float delta, min;
			float h = 0, s, v;

			min = Mathf.Min(Mathf.Min(rgb.r, rgb.g), rgb.b);
			v = Mathf.Max(Mathf.Max(rgb.r, rgb.g), rgb.b);
			delta = v - min;

			if (v == 0.0f)
				s = 0;
			else
				s = delta / v;

			if (s == 0)
				h = 0.0f;

			else
			{
				if (rgb.r == v)
					h = (rgb.g - rgb.b) / delta;
				else if (rgb.g == v)
					h = 2 + (rgb.b - rgb.r) / delta;
				else if (rgb.b == v)
					h = 4 + (rgb.b - rgb.g) / delta;

				h *= 60f;

				if (h < 0.0f)
					h = h + 360f;
			}

			return new HSV(h / 360.0f, s, v ); // (v / 255.0f)
		}

		public static Color HSVToRGB(HSV hsv)
		{
			float      hh, p, q, t, ff;
			int i;
			Color o = Color.white;

			if(hsv.s <= 0.0) {       // < is bogus, just shuts up warnings
				o.r = hsv.v;
				o.g = hsv.v;
				o.b = hsv.v;
				return o;
			}

			hh = hsv.h * 360.0f;

			if(hh >= 360.0f) 
				hh = 0.0f;
			hh /= 60.0f;

			i = (int)hsv.h;
			ff = hh - i;
			p = hsv.v * (1.0f - hsv.s);
			q = hsv.v * (1.0f - (hsv.s * ff));
			t = hsv.v * (1.0f - (hsv.s * (1.0f - ff)));

			switch(i) {
			case 0:
				o.r = hsv.v;
				o.g = t;
				o.b = p;
				break;
			case 1:
				o.r = q;
				o.g = hsv.v;
				o.b = p;
				break;
			case 2:
				o.r = p;
				o.g = hsv.v;
				o.b = t;
				break;

			case 3:
				o.r = p;
				o.g = q;
				o.b = hsv.v;
				break;
			case 4:
				o.r = t;
				o.g = p;
				o.b = hsv.v;
				break;
			case 5:
			default:
				o.r = hsv.v;
				o.g = p;
				o.b = q;
				break;
			}

			o.a = 1.0f;

			return o; 
		}

	}
}

