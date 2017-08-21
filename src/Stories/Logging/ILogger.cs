using System;

namespace Stories.Logging
{
    public interface ILogger
    {
        void Log(string message, Exception e = null);
    }
}
