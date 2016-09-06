﻿using System;
using System.IO;
using System.Threading;

namespace BackerV2
{
    public class FileLogger : Logger
    {
        private string LogFile = "BackerV2Logs";
        private Thread t;
        private volatile bool _shouldStop = false;
        public FileLogger()
        {
            createLogFile();
            startThread();
        }

        private void startThread()
        {
            t = new Thread(new ThreadStart(logToFile));
            t.Start();
        }

        private void logToFile()
        {
            Thread.Sleep(500);
            while (!_shouldStop)
            {
                string longString = "";
                for (int i = 0; i < writeBuffer.Count; i++)
                {
                    string temp;
                    writeBuffer.TryDequeue(out temp);
                    longString += temp + Environment.NewLine;
                }

                if (longString != "")
                {
                    try
                    {
                        StreamWriter writer = new StreamWriter(LogFile, true);
                        writer.Write(longString);
                        writer.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Something went bad in the logging class :( " + e);
                        isFunctional = false;
                        _shouldStop = true;
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
