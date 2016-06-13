using System;

namespace BackerV2
{
    class TcpLogger : Logger
    {
        //tcp logger is a test to see if just adding the class will do to replace FileLogger
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
