using NLog;
using System;

namespace Stories.Logging
{
    public class NLogAdaptor<T> : ILogger
    {
        private static readonly NLog.ILogger logger = LogManager.GetLogger(typeof(T).FullName);

        public void Log(string message, Exception e = null)
        {
            Log(LogLevel.Info, message, e);
        }

        public void Log(LogLevel logLevel, string message, Exception exception = null, params object[] args)
        {
            logger.Log(logLevel, exception, message, args);
        }
    }
}
