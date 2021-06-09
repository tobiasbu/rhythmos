using System.IO;
using UnityEngine;


namespace RhythmosEditor
{
    internal static class Useful
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
      
        public static string RecursiveFileExists(string fullpath)
        {
            if (File.Exists(fullpath))
            {
                string filename = Path.GetFileNameWithoutExtension(fullpath);
                string ext = Path.GetExtension(fullpath);
                string path = Path.GetDirectoryName(fullpath);

                return FileExistsRecursion(path, filename, ext, 1);
            }
            else
            {
                return fullpath;
            }
        }

        public static string ToStrippedString(this string[] value)
        {
            string str = "(";
            int len = value.Length;
            for (int i = 0; i < len; i += 1)
            {
                str += value[i];

                if (i >= len - 1)
                {
                    str += ")";
                } else
                {
                    str += ", ";
                }
            }
            return str;
        }

        public static bool IsAssetsPath(string path)
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

