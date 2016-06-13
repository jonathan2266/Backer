using System.Collections.Generic;

namespace BackerV2
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new FileLogger();
            environement envir = new environement(logger);
            ReadConfig config = new ReadConfig(logger);

            List<string> local = new List<string>();
            List<string> destination = new List<string>();
            envir.check();
            config.readSettings(local, destination);

            MainLoop loop = new MainLoop(local, destination,logger);
        }
    }
}
