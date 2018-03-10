using log4net;

namespace Log
{
    public static class LoggingExtension
    {
        
        /// <summary>
        /// Provides a logger.
        /// </summary>
        /// <typeparam name="T">The type of the logger.</typeparam>
        /// <param name="thing"></param>
        /// <returns></returns>
        public static ILog Logger<T>(this T thing)
        {
            var log = LogManager.GetLogger(typeof(T));
            return log;
        }
    }
}