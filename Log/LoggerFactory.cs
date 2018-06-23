using System;
using LeDragon.Log.Standard;
using log4net;

namespace Log
{
    public class LoggerFactory
    {
        public static ILogger Logger<T>()
        {
            return Logger(typeof(T));
        }

        public static ILogger Logger(Type type)
        {
            var log = LogManager.GetLogger(type);
            return new Logger(log);
        }
    }
}