using UnityEngine;

namespace RhythmosEditor.Utils
{
    internal class TextureUtility
    {
        static private void FillTextureGradient(Texture2D texture, int x, int y, int width, int height, Color from, Color to, bool lerpAlpha = false)
        {
            Color ncolor;
            for (int yy = 0; yy < height; yy++)
            {
                for (int xx = 0; xx < width; xx++)
                {
                    if (yy == 0)
                    {
                        ncolor = to;
                        ncolor.a = to.a;
                    }
                    else if (yy == height - 1)
                    {
                        ncolor = from;
                        ncolor.a = from.a;
                    }
                    else
                    {
                        if (from != to)
                        {
                            float pct = (float)((height - 1) - (float)yy) / (height - 1);
                            ncolor = Color.Lerp(from, to, pct);
                            if (lerpAlpha)
                            {
                                if (from.a != to.a)
                                    ncolor.a = Mathf.Lerp(from.a, to.a, pct);
                                else
                                    ncolor.a = from.a;
                            }
                            else
                            {
                                ncolor.a = 1;
                            }
                        }
                        else
                        {
                            ncolor = from;
                        }

                    }

                    texture.SetPixel(x + xx, (yy + (texture.height - height)) - y, ncolor);
                }
            }

            texture.Apply();
        }

        static private void FillTextureColor(Texture2D texture, int x, int y, int width, int height, Color color)
        {
            for (int yy = 0; yy < height; yy++)
            {
                for (int xx = 0; xx < width; xx++)
                {
                    texture.SetPixel(x + xx, (yy + (texture.height - height)) - y, color);
                }
            }
            texture.Apply();
        }

        static public Texture2D CreateTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(width, height);
            FillTextureColor(texture, 0, 0, width, height, color);
            texture.hideFlags = HideFlags.HideAndDontSave;
            return texture;
        }

        static public Texture2D CreateSpecialDegrade(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(width, height);
            HSV hsvColorA = HSV.ToHSV(color);
            HSV hsvColorB = hsvColorA;
            if (hsvColorA.s == 0)
            {
                if (hsvColorA.v >= 1f)
                {
                    hsvColorA.v = hsvColorA.v - 0.1f;
                }
                hsvColorB.v = hsvColorA.v * 0.85f;
            }
            else
            {
                hsvColorA.s = 0.1f;
                hsvColorA.v = 1f;
                hsvColorB.s = 0.1f;
                hsvColorB.v = 1f;
            }

            Color a = HSV.ToColor(hsvColorA);
            Color b = HSV.ToColor(hsvColorB);
            FillTextureColor(texture, 0, 0, width, 1, Color.gray);
            FillTextureGradient(texture, 0, 1, width, height - 3, a, b);
            FillTextureColor(texture, 0, height - 3, width, 3, Color.white);
            texture.hideFlags = HideFlags.HideAndDontSave;
            return texture;

        }

        static public Texture2D CreateFromBase64(string encodedText, HideFlags hide = HideFlags.HideAndDontSave)
        {
            byte[] array = System.Convert.FromBase64String(encodedText);
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(array);
            tex.hideFlags = hide;
            return tex;
        }

        internal static void Bucket(Texture2D texture, Color color) {
            for (int yy = 0; yy < texture.height; yy++)
            {
                for (int xx = 0; xx < texture.width; xx++)
                {

                    Color current = texture.GetPixel(xx, yy);
                    current.r = color.r;
                    current.g = color.g;
                    current.b = color.b;
                    texture.SetPixel(xx, yy, color);
                }
            }
            texture.Apply();
        }

        internal static Texture2D Clone(Texture2D texture, bool flipHorizontal) {
            Texture2D copy = new Texture2D(texture.width, texture.height);
            copy.hideFlags = texture.hideFlags;

            for (int yy = 0; yy < texture.height; yy++)
            {
                for (int xx = 0; xx < texture.width; xx++)
                {
                    Color color = texture.GetPixel(xx, yy);
                    if (flipHorizontal)
                    {
                        copy.SetPixel(texture.width - xx, yy, color);
                    }
                    else
                    {
                        copy.SetPixel(xx, yy, color);
                    }
                }
            }
            copy.Apply();

            return copy;
        }
    }
}
