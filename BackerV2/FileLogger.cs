using System;
using System.IO;
using System.Threading;

namespace BackerV2
{
    public class FileLogger : Logger
    {
        private Thread t;
        private volatile bool _shouldStop;
        public FileLogger()
        {
            createLogFile();
            startThread();
        }

        private void startThread()
        {
            _shouldStop = false;
            t = new Thread(new ThreadStart(logToFile));
            t.Start();
        }

        private void logToFile()
        {
            while (!_shouldStop)
            {
                string temp;
                writeBuffer.TryDequeue(out temp);
                if (temp != null)
                {
                    try
                    {
                        StreamWriter writer = new StreamWriter(LogFile, true); //next thread this plz :p yey threaded in a bad way =]
                        writer.WriteLine(temp);
                        writer.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something went bad in the logging class :( " + e);
                        isFunctional = false;
                        _shouldStop = true;
                        Console.WriteLine();
                    }
                }
                Thread.Sleep(10);
            }

        }
        private void createLogFile()
        {
            if (File.Exists(LogFile))
            {
                try
                {
                    File.Delete(LogFile);
                    File.Create(LogFile);
                    isFunctional = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Logging not possible");
                    Console.ReadLine();
                    isFunctional = false;
                }
            }
            else
            {
                try
                {
                    File.Create(LogFile);
                    isFunctional = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Logging not possible");
                    Console.ReadLine();
                    isFunctional = false;
                }
                
            }
        }
        public override void log(string text)
        {
            if (isFunctional)
            {
                writeBuffer.Enqueue(text);
            }
        }

        public override void stopLogging()
        {
            int currentCount = writeBuffer.Count;
            while (writeBuffer.Count > 0)
            {
                Thread.Sleep(50);
                if (currentCount > writeBuffer.Count)
                {
                    currentCount = writeBuffer.Count;
                }
                else
                {
                    break;
                }
            }
            _shouldStop = true;
        }
    }
}
