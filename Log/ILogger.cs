using System;
namespace Log
{
    public interface ILogger

    {
        void Debug(Object message);

        void DebugFormat(String format, params Object[] args);

        void Info(Object message);

        void InfoFormat(String format, params Object[] args);

        void Warn(Object message);

        void WarnFormat(String format, params Object[] args);

        void Error(Object message);

        void ErrorFormat(String format, params Object[] args);

        void Fatal(Object message);

        void FatalFormat(String format, params Object[] args);
    }
}