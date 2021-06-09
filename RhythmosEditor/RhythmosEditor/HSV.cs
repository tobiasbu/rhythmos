using UnityEngine;

namespace RhythmosEditor
{
    internal struct HSV
    {
        public float h;
        public float s;
        public float v;

        public HSV(float h, float s, float v)
        {
            this.h = h;
            this.s = s;
            this.v = v;
        }

        public override string ToString()
        {
            return string.Format("HSV(" + h + ", " + s + ", " + v + ")");
        }

        public static Color ToColor(HSV hsv)
        {
            float hh;
            Color o = Color.white;

            if (hsv.s <= 0.0f)
            {       // < is bogus, just shuts up warnings
                o.r = hsv.v;
                o.g = hsv.v;
                o.b = hsv.v;
                return o;
            }

            // convert normalized Hue to degree
            hh = hsv.h * 360.0f;
            if (hh >= 360.0f)
            {
                hh = 0.0f;
            }

            hh /= 60.0f;

            float chroma = hsv.v * hsv.s;
            float prime = hh % 6.0f;
            float x = chroma * (1.0f - Mathf.Abs(prime % 2.0f) - 1.0f);
            float m = hsv.v - chroma;

            //i = (int)hsv.h;
            //ff = hh - i;
            //p = hsv.v * (1.0f - hsv.s);
            //q = hsv.v * (1.0f - (hsv.s * ff));
            //t = hsv.v * (1.0f - (hsv.s * (1.0f - ff)));

            if (0 <= prime && prime < 1)
            {
                o.r = chroma;
                o.g = x;
                o.b = 0;
            }
            else if (1 <= prime && prime < 2)
            {

                o.r = x;
                o.g = chroma;
                o.b = 0;
            }
            else if (2 <= prime && prime < 3)
            {
                o.r = 0;
                o.g = chroma;
                o.b = x;
            }
            else if (3 <= prime && prime < 4)
            {
                o.r = 0;
                o.g = x;
                o.b = chroma;
            }
            else if (4 <= prime && prime < 5)
            {
                o.r = x;
                o.g = 0;
                o.b = chroma;
            }
            else if (5 <= prime && prime < 6)
            {
                o.r = chroma;
                o.g = 0;
                o.b = x;
            }
            else
            {
                o.r = 0;
                o.g = 0;
                o.b = 0;
            }

            o.r += m;
            o.g += m;
            o.b += m;
            o.a = 1.0f;

            return o;
        }

        internal Color ToRGB()
        {
            return HSV.ToColor(this);
        }
    }
}
