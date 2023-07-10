using System;
using System.IO;
using Domain.Logging;
using log4net;
using log4net.Config;

namespace Infrastructure.Logging
{
    public class LogManager : ILogManager
    {
        private static LogManager _logManager;

        static LogManager()
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            _logManager = new LogManager();
        }

        public static ILogger GetLogger<T>() where T : class
        {
            return _logManager.GetLogger(typeof (T));
        }

        public ILogger GetLogger(Type type)
        {
            var logger = log4net.LogManager.GetLogger(type);
            return new Logger(logger);
        }
    }
}