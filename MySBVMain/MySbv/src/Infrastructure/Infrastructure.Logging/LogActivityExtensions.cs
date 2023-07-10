using System;
using Domain.Logging;

namespace Infrastructure.Logging
{
    public static class LogActivityExtensions
    {
        public static IDisposable Activity(this ILogger log, string format, params object[] args)
        {
            return new LoggingActivity(log, String.Format(format, args));
        }
    }
}