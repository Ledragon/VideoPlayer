using log4net;

namespace LeDragon.Log.Standard
{
    public static class LoggingExtension
    {
        
        /// <summary>
        /// Provides a logger.
        /// </summary>
        /// <typeparam name="T">The type of the logger.</typeparam>
        /// <param name="thing"></param>
        /// <returns></returns>
        public static ILogger Logger<T>(this T thing)
        {
            var log = LogManager.GetLogger(typeof(T));
            return new Logger(log);
        }
    }
}