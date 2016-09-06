using System;
using System.Collections.Generic;
using System.IO;

namespace BackerV2
{
    public class ReadConfig //could be abstract or with interface to be able to read different files
    {
        private string configFile = "";
        Ilogger logger;
        public ReadConfig(Ilogger logger, string configFile)
        {
            this.logger = logger;
            this.configFile = configFile;
        }
        public void readSettings(List<string> line1, List<string> line2)
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
                            line1.Add(_local);
                            line2.Add(_destination);
                        }
                        else
                        {
                            try
                            {
                                FileAttributes att = File.GetAttributes(_local);
                                line1.Add(_local);
                                line2.Add(_destination);
                            }
                            catch (Exception)
                            {
                                logger.log(_local + " and " + _destination + " something is wrong ");
                            }
                        }
                    }
                }
                contents = r.ReadLine();
            }

            r.Close();
        }
    }
}
