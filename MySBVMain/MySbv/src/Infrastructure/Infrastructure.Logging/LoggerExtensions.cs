using System;
using Domain.Logging;

namespace Infrastructure.Logging
{
    public static class LoggerExtensions
    {
        public static void Debug(this ILogger log, Action method, string message)
        {
            method.Invoke();

            if (log.IsDebugEnabled)
            {
                log.Debug(message);
            }
        }

        public static void Info(this ILogger log, Action method, string message)
        {
            method.Invoke();

            if (log.IsInfoEnabled)
            {
                log.Debug(message);
            }
        }

        public static void Warn(this ILogger log, Action method, string message)
        {
            method.Invoke();

            if (log.IsWarnEnabled)
            {
                log.Debug(message);
            }
        }

        public static void Error(this ILogger log, Action method, string message)
        {
            method.Invoke();

            if (log.IsErrorEnabled)
            {
                log.Debug(message);
            }
        }

        public static void Fatal(this ILogger log, Action method, string message)
        {
            method.Invoke();

            if (log.IsFatalEnabled)
            {
                log.Debug(message);
            }
        }
    }
}