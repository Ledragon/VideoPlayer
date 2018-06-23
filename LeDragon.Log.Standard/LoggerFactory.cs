using log4net;
using System;

namespace LeDragon.Log.Standard
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
