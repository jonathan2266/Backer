using System;
using System.Collections.Generic;
using System.IO;

namespace Backer
{
    public class archiver
    {
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
                DirectoryCopy(_local[i], _destination[i] + "\\" + fixedDate, true);   
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
                file.CopyTo(temppath, false);
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
    }
}
