using System;
using System.IO;
using System.Text;

namespace BackerV2
{
    public class environement
    {
        private Ilogger logger;

        public environement(Ilogger logger)
        {
            this.logger = logger;
        }

        public void check()
        {
            if (!File.Exists("backer.cnf"))
            {
                try
                {
                    StreamWriter w = new StreamWriter("backer.cnf", true, Encoding.ASCII);
                    w.Write("#everything is put between \"\" Starting with the localDir and on the next line the destinationDir. Text between #..# will be ingnored#" + Environment.NewLine);
                    w.Close();
                }
                catch (Exception e)
                {
                    logger.log(e.Message);
                }
            }
        }
    }
}
