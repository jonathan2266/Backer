using System;
using System.Collections.Generic;
using System.IO;

namespace BackerV2
{
    public class Archiver
    {
        Ilogger logger;
        private List<string> _local = new List<string>();
        private List<string> _destination = new List<string>();
        public Archiver(Ilogger logger)
        {
            this.logger = logger;
        }
        public void addLocalDestPair(string local, string destination)
        {
            _local.Add(local);
            _destination.Add(destination);
        }
        public void transferFiles()
        {
            string fixedDate = DateFixer.fix(DateTime.Now);

            for (int i = 0; i < _local.Count; i++)
            {
                try
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
                catch (Exception e)
                {
                    logger.log(e.Message);
                }
            }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                logger.log("Source directory does not exist or could not be found: "
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
                    logger.log("File not copied " + e.Message);
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
                logger.log("Failed to copy file " + e.Message);
            }

        }
    }
}
