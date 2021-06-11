using RhythmosEngine;
using UnityEditor;
using UnityEngine;

namespace RhythmosEditor
{
    internal class EditorAssetLoader : IAssetLoader
    {
        public AudioClip LoadClip(string path)
        {
           return (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
    }
}
