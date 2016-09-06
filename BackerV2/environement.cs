using System;
using System.IO;
using System.Text;

namespace BackerV2
{
    public class environement
    {
        private Ilogger logger;
        private string startComment = "";
        private string fileToCheck = "";
        public environement(Ilogger logger, string startComment, string fileToCheck)
        {
            this.logger = logger;
            this.startComment = startComment;
            this.fileToCheck = fileToCheck;
        }

        public void check()
        {
            if (!File.Exists(fileToCheck))
            {
                try
                {
                    StreamWriter w = new StreamWriter(fileToCheck, true, Encoding.ASCII);
                    w.Write(startComment + Environment.NewLine);
                    w.Close();
                }
                catch (Exception e)
                {
                    logger.log(e.Message);
                }
                Environment.Exit(1);
            }
        }
    }
}
