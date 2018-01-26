using Microsoft.Extensions.Logging;
using System;

namespace Stories.Logging
{
    public class LoggerAdapter<T> : ILogger<T>
    {
        private readonly Microsoft.Extensions.Logging.ILogger adaptee;

        public LoggerAdapter(ILoggerFactory factory)
        {
            adaptee = factory.CreateLogger<T>();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return adaptee.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return adaptee.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            adaptee.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
