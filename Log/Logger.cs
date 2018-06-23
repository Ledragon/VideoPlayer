using System;
using log4net;

namespace Log
{
    public class Logger : ILogger
    {
        private readonly ILog _log;

        public Logger(ILog log)
        {
            this._log = log;
        }

        public void Debug(Object message)
        {
            if (this._log.IsDebugEnabled)
            {
                this._log.Debug(message);
            }
        }

        public void DebugFormat(String format, params Object[] args)
        {
            if (this._log.IsDebugEnabled)
            {
                this._log.DebugFormat(format, args);
            }
        }

        public void Info(Object message)
        {
            if (this._log.IsInfoEnabled)
            {
                this._log.Info(message);
            }
        }

        public void InfoFormat(String format, params Object[] args)
        {
            if (this._log.IsInfoEnabled)
            {
                this._log.InfoFormat(format, args);
            }
        }

        public void Warn(Object message)
        {
            if (this._log.IsWarnEnabled)
            {
                this._log.Warn(message);
            }
        }

        public void WarnFormat(String format, params Object[] args)
        {
            if (this._log.IsWarnEnabled)
            {
                this._log.WarnFormat(format, args);
            }
        }

        public void Error(Object message)
        {
            if (this._log.IsErrorEnabled)
            {
                this._log.Error(message);
            }
        }

        public void ErrorFormat(String format, params Object[] args)
        {
            if (this._log.IsErrorEnabled)
            {
                this._log.ErrorFormat(format, args);
            }
        }

        public void Fatal(Object message)
        {
            if (this._log.IsFatalEnabled)
            {
                this._log.Fatal(message);
            }
        }

        public void FatalFormat(String format, params Object[] args)
        {
            if (this._log.IsFatalEnabled)
            {
                this._log.FatalFormat(format, args);
            }
        }
    }
}