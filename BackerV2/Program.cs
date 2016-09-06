using System.Collections.Generic;

namespace BackerV2
{
    class Program
    {
        static void Main(string[] args)
        {
            string startComment = "#everything is put between \"\" Starting with the localDir and on the next line the destinationDir. Text between #..# will be ingnored#";
            string fileName = "backer.cnf";

            Logger logger = new FileLogger();
            environement envir = new environement(logger, startComment, fileName);
            ReadConfig config = new ReadConfig(logger, fileName);

            List<string> local = new List<string>();
            List<string> destination = new List<string>();
            envir.check();
            config.readSettings(local, destination);

            MainLoop loop = new MainLoop(local, destination, logger);
        }
    }
}
