using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BackerV2
{
    public class MainLoop
    {
        Archiver[] _archiver;
        List<Thread> threadList = new List<Thread>();
        private List<string> local;
        private List<string> destination;
        private DriveInfo[] allDrives;
        Ilogger __logger;

        public MainLoop(List<string> local, List<string> destination, Ilogger logger)
        {
            this.local = local;
            this.destination = destination;
            __logger = logger;
            try
            {
                allDrives = DriveInfo.GetDrives();
            }
            catch (System.Exception e)
            {
                __logger.log("Drive access failure: " + e.Message);
            }
            prepareThreads();
            
        }

        private void prepareThreads( )
        {
            _archiver = new Archiver[allDrives.Length];

            for (int i = 0; i < local.Count; i++)
            {
                bool added = false;
                for (int j = 0; j < allDrives.Length; j++)
                {
                    if (local[i].Contains(allDrives[j].Name))
                    {
                        if (_archiver[j] == null)
                        {
                            _archiver[j] = new Archiver(__logger);
                        }
                        _archiver[j].addLocalDestPair(local[i], destination[i]);
                        added = true;
                    }
                }
                if (added == false)
                {
                    __logger.log(local[i] + " Drive not found on local pc");
                }
            }

            for (int i = 0; i < _archiver.Length; i++)
            {
                if (_archiver[i] != null)
                {
                    Thread thread = new Thread(new ThreadStart(_archiver[i].transferFiles));
                    threadList.Add(thread);
                }
            }

            for (int i = 0; i < threadList.Count; i++)
            {
                threadList[i].Start();
            }

            int deadCount = 0;
            while (true)
            {
                for (int i = 0; i < threadList.Count; i++)
                {
                    if (!threadList[i].IsAlive)
                    {
                        deadCount++;
                    }
                }
                if (deadCount == threadList.Count)
                {
                    break;
                }
                else
                {
                    deadCount = 0;
                    Thread.Sleep(10);
                }
            }
            __logger.stopLogging();
        }
    }
}
