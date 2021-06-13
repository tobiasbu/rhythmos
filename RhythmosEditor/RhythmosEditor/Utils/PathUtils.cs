using System.IO;
using UnityEngine;

namespace RhythmosEditor
{
    internal static class PathUtils
	{
        private static string FileExistsRecursion(string path, string filename, string extension, int index)
        {
            string newFileName = string.Concat(filename, " ", index.ToString(), extension);
            string fullNewPath = Path.Combine(path, newFileName);

            if (File.Exists(fullNewPath))
            {
                return FileExistsRecursion(path, filename, extension, index + 1);
            }
            
            return fullNewPath;
        }

        public static bool IsAssetsAbsolutePath(string path)
        {
            string[] assetsPath = Application.dataPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string[] pathToCheck = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (assetsPath.Length > pathToCheck.Length)
            {
                return false;
            }

            for (int i = 0; i < assetsPath.Length; i += 1)
            {
                if (assetsPath[i] != pathToCheck[i])
                {
                    return false;
                }
            }
            return true;
        }
	}
}

