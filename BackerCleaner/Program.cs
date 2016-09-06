using BackerV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackerCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            string startComment = "# everything with # will be ignored First line Folder to check second line amount of backups to keep" + Environment.NewLine + "# /home/user/backups/" + Environment.NewLine + "# 5";
            string fileToCheck = "Cleaner.cnf";

            Logger logger = new FileLogger();
            environement envir = new environement(logger, startComment, fileToCheck);
            ReadConfig config = new ReadConfig(logger, fileToCheck);

            List<string> folder = new List<string>();
            List<string> amountS = new List<string>();
            List<int> amountI = new List<int>();
            envir.check();
            config.readSettings(folder, amountS);

            for (int i = 0; i < amountS.Count; i++)
            {
                try
                {
                    amountI.Add(Convert.ToInt32(amountS[i]));
                }
                catch (InvalidCastException e)
                {
                    logger.log("Something is wrong with config file: " + e);
                    logger.stopLogging();
                    Environment.Exit(1);
                }
            }
            amountS = null;

            Cleaner c = new Cleaner(logger, folder, amountI);
            c.StartRemoving();
            Environment.Exit(0);
        }
    }
}
