using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PACS_Utils
{
    public static class FileManagement
    {
        public static void EncryptFile(string originFile, string encryptedPath,
            Dictionary<char, string> encryptedLetters)
        {
            var iStream = new FileStream(originFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sr = new StreamReader(iStream);

            using (var sw = File.CreateText(encryptedPath))
            {
                var res = sr.Read();
                while (res != -1)
                {
                    sw.Write(encryptedLetters[(char) res]);
                    res = sr.Read();
                }

                sw.Flush();
                sw.Close();
            }

            sr.Close();
            iStream.Close();
        }

        public static void JoinTxtFiles(IEnumerable<string> filePaths, string finalPath)
        {
            using (var output = File.Create(finalPath))
            {
                foreach (var filePath in filePaths)
                    using (var inp = File.OpenRead(filePath))
                    {
                        inp.CopyTo(output);
                    }
            }
        }

        public static void CreateFile(string path, string content, bool deleteIfExists)
        {
            if (File.Exists(path) && deleteIfExists) File.Delete(path);
            using (var tw = File.CreateText(path))
            {
                tw.Write(content);
                tw.Flush();
            }
        }

        public static bool IsContentEqual(string path1, string path2)
        {
            return File.ReadLines(path1).SequenceEqual(File.ReadLines(path2));
        }

        public static bool IsContentEqual(params string[] paths)
        {
            var equalFiles = 0;

            foreach (var path in paths)
                if (File.ReadLines(paths[0]).SequenceEqual(File.ReadLines(path)))
                    equalFiles++;

            return equalFiles == paths.Length;
        }
    }
}