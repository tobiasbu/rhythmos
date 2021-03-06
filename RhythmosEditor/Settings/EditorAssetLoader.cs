using UnityEngine;
using UnityEditor;
using RhythmosEngine;

namespace RhythmosEditor.Settings
{
    internal class EditorAssetLoader : IAssetLoader
    {
        public AudioClip LoadClip(string path)
        {
            return (AudioClip)AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip));
        }
    }
}
