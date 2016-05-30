using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Backer
{
    class Program
    {
        static string configFile = "backer.cnf";
        static bool isError = false;
        static void Main(string[] args)
        {
            List<string> local = new List<string>();
            List<string> destination = new List<string>();
            archiver[] _archiver;
            List<Thread> threadList = new List<Thread>();
            checkEnvironement();

            readFileContents(local, destination);

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            _archiver = new archiver[allDrives.Length];

            for (int i = 0; i < local.Count; i++)
            {
                for (int j = 0; j < allDrives.Length; j++)
                {
                    if (local[i].Contains(allDrives[j].Name))
                    {
                        if (_archiver[j] == null)
                        {
                            _archiver[j] = new archiver();
                        }
                        _archiver[j].addLocalDestPair(local[i], destination[i]);
                    }
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

            if (isError)
            {
                Console.ReadLine();
            }
            
        }

        private static void readFileContents(List<string> local, List<string> destination)
        {
            StreamReader r = new StreamReader(configFile);
            string contents = r.ReadLine();
            bool onEven = true;
            string _local = "";
            string _destination = "";

            while (contents != null)
            {
                char[] charContent = contents.ToCharArray();

                bool containsLink = false;
                bool containsHash = false;
                for (int i = 0; i < charContent.Length; i++)
                {
                    if (charContent[i] == '"')
                    {
                        containsLink = true;
                    }
                    if (charContent[i] == '#')
                    {
                        containsHash = true;
                    }
                }

                if (containsLink && !containsHash)
                {
                    if (onEven == true)
                    {
                        _local = contents;
                        _local = _local.Remove(0, 1);
                        _local = _local.Remove(_local.Length - 1, 1);
                        onEven = false;
                    }
                    else
                    {
                        _destination = contents;
                        _destination = _destination.Remove(0, 1);
                        _destination = _destination.Remove(_destination.Length - 1, 1);
                        onEven = true;

                        if (Directory.Exists(_local) && Directory.Exists(_destination))
                        {
                            local.Add(_local);
                            destination.Add(_destination);
                        }
                        else
                        {
                            isError = true;
                            Console.WriteLine(_local + " and " + _destination + " something is wrong ");
                        }
                    }
                }
                contents = r.ReadLine();
            }

            r.Close();
        }

        private static void checkEnvironement()
        {
            if (!File.Exists("backer.cnf"))
            {
                try
                {
                    StreamWriter w = new StreamWriter("backer.cnf",true,Encoding.ASCII);
                    w.Write("#everything is put between \"\" Starting with the localDir and on the next line the destinationDir. Text between #..# will be ingnored#" + Environment.NewLine);
                    w.Close();
                }
                catch (Exception e)
                {
                    isError = true;
                    Console.WriteLine(e);
                }
            }
            
        }
    }
}
