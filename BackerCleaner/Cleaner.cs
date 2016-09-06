using BackerV2;
using System;
using System.Collections.Generic;
using System.IO;

namespace BackerCleaner
{
    public class Cleaner
    {
        private Ilogger logger;
        private List<string> folder;
        private List<int> folderInfo;
        public Cleaner(Ilogger logger, List<string> folder, List<int> folderInfo)
        {
            this.logger = logger;
            this.folder = folder;
            this.folderInfo = folderInfo;
        }
        public void StartRemoving()
        {
            for (int i = 0; i < folder.Count; i++)
            {
                List<string> Fulldirs;
                List<string> dirNames;
                List<DateTime> dates;
                int foldersToRemove;

                getDirs(i, out Fulldirs, out dirNames);
                dirNamesToDates(dirNames, out dates);

                foldersToRemove = Fulldirs.Count - folderInfo[i];
                if (foldersToRemove <= 0)
                {
                    break;
                }
                //maybe filter out stuff with max date
                for (int a = 0; a < dates.Count; a++)
                {
                    if (dates[a] == DateTime.MaxValue)
                    {
                        logger.log("Warning not removing folder: " + Fulldirs[a]);
                        dates.RemoveAt(a);
                        Fulldirs.RemoveAt(a);
                        dirNames.RemoveAt(a);
                        a--;
                    }
                }

                removeOldest(Fulldirs, dates, foldersToRemove);
            }
        }

        private void removeOldest(List<string> fulldirs, List<DateTime> dates, int foldersToRemove)
        {
            for (int i = 0; i < foldersToRemove; i++)
            {
                int removeNr = oldestDate(dates);
                try
                {
                    Directory.Delete(fulldirs[removeNr], true);
                }
                catch (IOException e)
                {

                    logger.log("tried to remove Folder: " + e);
                    break;
                }
                
                dates.RemoveAt(i);
                fulldirs.RemoveAt(i);
            }
        }

        private int oldestDate(List<DateTime> dates)
        {
            int oldest = 0;

            for (int i = 0; i < dates.Count; i++)
            {
                if (dates[oldest] > dates[i])
                {
                    oldest = i;
                }
            }
            return oldest;
        }

        private void dirNamesToDates(List<string> dirNames, out List<DateTime> dates)
        {
            dates = new List<DateTime>();
            for (int j = 0; j < dirNames.Count; j++)
            {
                dates.Add(DateFixer.FolderToDateFormat(dirNames[j]));
            }
        }

        private void getDirs(int i, out List<string> Fulldirs, out List<string> dirNames)
        {
            Fulldirs = new List<string>();
            dirNames = new List<string>();

            string[] tempFulldirs = Directory.GetDirectories(folder[i]); // including path
            for (int m = 0; m < tempFulldirs.Length; m++)
            {
                Fulldirs.Add(tempFulldirs[m]);
            }
            for (int k = 0; k < Fulldirs.Count; k++)
            {
                dirNames.Add(Path.GetFileName(Fulldirs[k]));
            }
        }
    }
}
