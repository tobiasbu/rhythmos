using UnityEngine;

namespace RhythmosEngine
{
    /// <summary>
    /// Provides a way to load assets.
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary>
        /// Load <c>AudioClip</c> by given path.
        /// </summary>
        /// <param name="path">Path to <c>AudioClip</c> asset</param>
        /// <returns>AudioClip asset</returns>
        AudioClip LoadClip(string path);
    }
}
