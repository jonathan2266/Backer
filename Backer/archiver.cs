using System;
using System.Collections.Generic;
using System.IO;

namespace Backer
{
    public class archiver
    {
        private bool isError = false;
        private List<string> _local = new List<string>();
        private List<string> _destination = new List<string>();
        public void addLocalDestPair(string local, string destination)
        {
            _local.Add(local);
            _destination.Add(destination);
        }
        public void transferFiles()
        {
            DateTime localDate = DateTime.Now;
            string fixedDate = localDate.ToString();
            fixedDate = fixedDate.Replace('/', '_');
            fixedDate = fixedDate.Replace(':', '-');
            fixedDate = fixedDate.Replace(" ", "__");

            for (int i = 0; i < _local.Count; i++)
            {
                FileAttributes att = File.GetAttributes(_local[i]);

                if (att.HasFlag(FileAttributes.Directory))
                {
                    DirectoryCopy(_local[i], _destination[i] + "\\" + fixedDate, true);
                }
                else
                {
                    FileCopy(_local[i], _destination[i] + "\\" + fixedDate);
                }
                   
            }

            if (isError)
            {
                Console.WriteLine("Reached end of a thread");
                Console.ReadLine();
            }
        }
        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                try
                {
                    file.CopyTo(temppath, false);
                }
                catch (Exception e)
                {
                    isError = true;
                    Console.WriteLine("File not copied " + e);
                }
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        private void FileCopy(string source, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
            try
            {
                File.Copy(source, Path.Combine(destination, Path.GetFileName(source)));
            }
            catch (Exception e)
            {
                isError = true;
                Console.WriteLine("Failed to copy file " + e);
            }
            
        }
    }
}
