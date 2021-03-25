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

        public static void DecryptFile(string encryptedPath, string decryptedPath,
            Dictionary<char, string> encryptedLetters)
        {
            var iStream = new FileStream(encryptedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var sr = new StreamReader(iStream);

            using (var sw = File.CreateText(decryptedPath))
            {
                var res = sr.Read();
                var code = "";
                while (res != -1)
                {
                    code += (char) res;

                    if (code.Length == 3)
                    {
                        var letter = encryptedLetters.FirstOrDefault(x => x.Value.Equals(code));
                        sw.Write(letter.Key);
                        code = @"";
                    }

                    //sw.Write(encryptedLetters[(char) res]);
                    res = sr.Read();
                }

                sw.Flush();
                sw.Close();
            }

            sr.Close();
            iStream.Close();
        }

        public static void ReplaceString(string filename, string search, string replace)
        {
            var sr = new StreamReader(filename);
            var rows = Regex.Split(sr.ReadToEnd(), "\r\n");
            sr.Close();

            var sw = new StreamWriter(filename);
            for (var i = 0; i < rows.Length; i++)
            {
                if (rows[i].Contains(search)) rows[i] = rows[i].Replace(search, replace);

                sw.WriteLine(rows[i]);
            }

            sw.Close();
        }

        public static void JoinTxtFiles(IEnumerable<string> filePaths, string finalPath)
        {
            if (File.Exists(finalPath)) { File.Delete(finalPath); }
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

            string tempFolder;
            var i = 0;
            do
            {
                i++;
                tempFolder = Path.Combine(Application.StartupPath, $@"tempfolder{i}");
            } while (Directory.Exists(tempFolder));

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
            var strFiles = Directory.GetFiles(extractDirectory, "*", SearchOption.AllDirectories).ToList();
            foreach (var fichero in strFiles) File.Delete(fichero);

            //if (Directory.Exists(extractDirectory)) Directory.Delete(extractDirectory);
            System.IO.Compression.ZipFile.ExtractToDirectory(fileToUnzip, extractDirectory);
        }

        public static bool IsContentEqual(string path1, string path2)
        {
            try
            {
                return File.Exists(path1) && File.Exists(path2) &&
                       File.ReadLines(path1).SequenceEqual(File.ReadLines(path2));
            }
            catch
            {
                return false;
            }
        }

        public static bool IsContentEqual(params string[] paths)
        {
            try
            {
                return paths.Count(path => File.ReadLines(paths[0]).SequenceEqual(File.ReadLines(path))) ==
                       paths.Length;
            }
            catch
            {
                return false;
            }
        }
    }
}