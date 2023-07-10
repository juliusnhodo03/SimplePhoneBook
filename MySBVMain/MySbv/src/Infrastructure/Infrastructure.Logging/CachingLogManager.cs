using System;
using System.Collections.Generic;
using Domain.Logging;

namespace Infrastructure.Logging
{
    public class CachingLogManager : ILogManager
    {
        private static readonly IDictionary<Type, ILogger> LoggerMap = new Dictionary<Type, ILogger>();
        private readonly ILogManager _logManager;

        public CachingLogManager(ILogManager logManager)
        {
            _logManager = logManager;
        }

        public ILogger GetLogger(Type type) 
        {
            ILogger logger;
            if (!LoggerMap.TryGetValue(type, out logger))
            {
                logger = _logManager.GetLogger(type);
                LoggerMap[type] = logger;
            }

            return logger;
        }
    }
}