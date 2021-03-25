using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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

        public static void CreateDirectory(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        public static void CreateFile(string path, string content)
        {
            if (File.Exists(path)) File.Delete(path);
            using (var tw = File.Create(path))
            {
                var bytes = Encoding.UTF8.GetBytes(content);

                tw.Write(bytes, 0, bytes.Length);

                tw.Flush();
                tw.Close();
            }
        }

        public static void ZipFile(string directoryToZip, string zipPath)
        {
            if (File.Exists(zipPath)) File.Delete(zipPath);
            System.IO.Compression.ZipFile.CreateFromDirectory(directoryToZip, zipPath);
        }

        public static void ZipFile(string zipPath, IEnumerable<string> files)
        {
            if (File.Exists(zipPath)) File.Delete(zipPath);

            var tempFolder = Path.Combine(Application.StartupPath, @"tempfolder");
            foreach (var file in files)
                if (File.Exists(file))
                    File.Copy(file, tempFolder);

            System.IO.Compression.ZipFile.CreateFromDirectory(tempFolder, zipPath);

            Directory.Delete(tempFolder);
        }

        public static IOrderedEnumerable<string> GetFilteredFileList(string filesDirectory, Regex filesReg,
            Regex orderReg)
        {
            return Directory.GetFiles(filesDirectory).Where(s => filesReg.IsMatch(s))
                .OrderBy(file => orderReg.Match(file).Value);
        }

        public static void UnzipFile(string fileToUnzip, string extractDirectory)
        {
            if (Directory.Exists(extractDirectory)) Directory.Delete(extractDirectory);
            System.IO.Compression.ZipFile.ExtractToDirectory(fileToUnzip, extractDirectory);
        }

        public static bool IsContentEqual(string path1, string path2)
        {
            try
            {
                if (File.Exists(path1) && File.Exists(path2))
                    return File.ReadLines(path1).SequenceEqual(File.ReadLines(path2));

                return false;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsContentEqual(params string[] paths)
        {
            return paths.Count(path => File.ReadLines(paths[0]).SequenceEqual(File.ReadLines(path))) == paths.Length;
        }
    }
}