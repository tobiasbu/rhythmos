using UnityEngine;

namespace RhythmosEngine
{
    internal class ResourceAssetLoader : IAssetLoader
    {
        public AudioClip LoadClip(string path)
        {
            string runtimePath = path.Replace("Assets/Resources/", "");
            runtimePath = runtimePath.Remove(runtimePath.Length - 4, 4);
            return (AudioClip)Resources.Load(runtimePath, typeof(AudioClip));
        }
    }
}
