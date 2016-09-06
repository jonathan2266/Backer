using System.Collections.Concurrent;

namespace BackerV2
{
    abstract public class Logger : Ilogger
    {
        protected bool isFunctional;
        protected ConcurrentQueue<string> writeBuffer = new ConcurrentQueue<string>();

        public abstract void log(string text);

        public abstract void stopLogging();
    }
}
