using LeDragon.Log.Standard;
using System;
using System.Diagnostics;

namespace LeDragon.Log.Standard
{
    public static class PerfLogger
    {
        private static readonly ILogger _logger;

        static PerfLogger()
        {
            _logger = LoggerFactory.Logger(typeof(PerfLogger));
        }

        public static void Monitor<T>(this T o, Action method, String methodName)
        {
            var sw = new Stopwatch();
            sw.Start();
            method.Invoke();
            sw.Stop();
            _logger.Info(new {Method = methodName, Class = o.GetType().Name, Duration = sw.ElapsedMilliseconds});
        }

        public static TReturn Monitor<T, TReturn>(this T o, Func<TReturn> method, String methodName)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = method.Invoke();
            sw.Stop();
            _logger.Info(new {Method = methodName, Class = o.GetType().Name, Duration = sw.ElapsedMilliseconds});
            return result;
        }
    }
}