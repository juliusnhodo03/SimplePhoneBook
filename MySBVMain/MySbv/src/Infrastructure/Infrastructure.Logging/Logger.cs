using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using Domain.Logging;
using log4net;

namespace Infrastructure.Logging
{
    [Export(typeof(ILogger))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Logger : ILogger
    {
        #region Fields

        private static ILog _log;

        #endregion

        #region Class Initializer

        public Logger()
        {
            GlobalContext.Properties["host"] = Environment.MachineName;
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            GlobalContext.Properties["version"] = version.ToString();
        }

        public Logger(ILog log)
        {
            _log = log;
            GlobalContext.Properties["host"] = Environment.MachineName;
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            GlobalContext.Properties["version"] = version.ToString();
        }

        #endregion

        #region ILogger Members

        public void Debug(object message)
        {
            if(_log.IsDebugEnabled)
                _log.Debug(message);
        }

        public void Debug(object message, Exception exception)
        {
            if(_log.IsDebugEnabled)
            _log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (_log.IsDebugEnabled)
            _log.DebugFormat(format, args);
        }

        public void DebugFormat(string format, object arg0)
        {
            if (_log.IsDebugEnabled)
            _log.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            if (_log.IsDebugEnabled)
            _log.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_log.IsDebugEnabled)
            _log.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_log.IsDebugEnabled)
            _log.DebugFormat(provider, format, args);
        }

        public void Info(object message)
        {
            if (_log.IsInfoEnabled)
            _log.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            if (_log.IsInfoEnabled)
            _log.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (_log.IsInfoEnabled)
            _log.InfoFormat(format, args);
        }

        public void InfoFormat(string format, object arg0)
        {
            if (_log.IsInfoEnabled)
            _log.InfoFormat(format, arg0);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            if (_log.IsInfoEnabled)
            _log.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_log.IsInfoEnabled)
            _log.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_log.IsInfoEnabled)
            _log.InfoFormat(provider, format, args);
        }

        public void Warn(object message)
        {
            if (_log.IsWarnEnabled)
            _log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            if (_log.IsWarnEnabled)
            _log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (_log.IsWarnEnabled)
            _log.WarnFormat(format, args);
        }

        public void WarnFormat(string format, object arg0)
        {
            if (_log.IsWarnEnabled)
            _log.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            if (_log.IsWarnEnabled)
            _log.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_log.IsWarnEnabled)
            _log.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_log.IsWarnEnabled)
            _log.WarnFormat(provider, format, args);
        }

        public void Error(object message)
        {
            if (_log.IsErrorEnabled)
            _log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            if (_log.IsErrorEnabled)
            _log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (_log.IsErrorEnabled)
            _log.ErrorFormat(format, args);
        }

        public void ErrorFormat(string format, object arg0)
        {
            if (_log.IsErrorEnabled)
            _log.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            if (_log.IsErrorEnabled)
            _log.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_log.IsErrorEnabled)
            _log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_log.IsErrorEnabled)
            _log.ErrorFormat(provider, format, args);
        }

        public void Fatal(object message)
        {
            if (_log.IsFatalEnabled)
            _log.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            if (_log.IsFatalEnabled)
            _log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (_log.IsFatalEnabled)
            _log.FatalFormat(format, args);
        }

        public void FatalFormat(string format, object arg0)
        {
            if (_log.IsFatalEnabled)
            _log.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            if (_log.IsFatalEnabled)
            _log.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            if (_log.IsFatalEnabled)
            _log.FatalFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            if (_log.IsFatalEnabled)
            _log.FatalFormat(provider, format, args);
        }

        public void LogException(Exception exception)
        {
            string excMsg = exception.InnerException != null
                ? exception.InnerException.ToString()
                : exception.StackTrace;

            if (_log.IsErrorEnabled)
                _log.Error(
                    string.Format(CultureInfo.InvariantCulture, "MESSAGE\n{0}\nSTACKTRACE\n{1}\nINNER EXCEPTION{2}",
                        exception.Message, exception.StackTrace, excMsg), exception);
        }

        public bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _log.IsWarnEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _log.IsFatalEnabled; }
        }

        public IDisposable PushActivity(string activityName)
        {
            return ThreadContext.Stacks["activity"].Push(activityName);
        }

        #endregion

    }
}