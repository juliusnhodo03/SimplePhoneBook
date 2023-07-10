using System;
using Domain.Logging;

namespace Infrastructure.Logging
{
    internal class LoggingActivity : IDisposable
    {
        private readonly string _activityName;
        private readonly ILogger _log;
        private readonly IDisposable _scope;

        public LoggingActivity(ILogger log, string activityName)
        {
            _log = log;
            _activityName = activityName;

            _scope = _log.PushActivity(activityName);
            log.DebugFormat(">> Entering activity [{0}]", activityName);
        }

        public void Dispose()
        {
            _log.DebugFormat("<< Leaving activity [{0}]", _activityName);
            _scope.Dispose();
        }
    }
}