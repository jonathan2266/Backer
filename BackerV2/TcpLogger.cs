using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackerV2
{
    class TcpLogger : Logger
    {
        //tcp logger is a test to see ifjust adding the class will do to replace FileLogger
        public override void log(string text)
        {
            throw new NotImplementedException();
        }

        public override void stopLogging()
        {
            throw new NotImplementedException();
        }
    }
}
