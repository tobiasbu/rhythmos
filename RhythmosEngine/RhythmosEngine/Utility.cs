//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;

namespace RhythmosEngine
{
    internal class Utility
    {
        public static Color ParseColor(string col)
        {
            Color output = new Color();
            if (col.StartsWith("RGBA("))
            {
                col = col.Remove(0, 5);
                col = col.Remove(col.Length - 1, 1);

            }

            string[] strs = col.Split(","[0]);

            for (int i = 0; i < 4; i++)
            {
                output[i] = System.Single.Parse(strs[i]);
            }
            return output;
        }
    }
}

