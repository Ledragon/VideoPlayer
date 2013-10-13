using System;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Log
{
    public class LoggingSystemManager
    {
        public void SetPath(String path)
        {
            //_writer = new StreamWriter(path);

            //_path = path;
            PatternLayout patternLayout = new PatternLayout
            {
                ConversionPattern = "[%date] - [%logger] - %level  - %message%newline"
            };
            patternLayout.ActivateOptions();

            RollingFileAppender rollingFileAppender = new RollingFileAppender
            {
                File = path,
                Layout = patternLayout,
                MaximumFileSize = "5MB",
                MaxSizeRollBackups = 2,
                PreserveLogFileNameExtension = true,
                RollingStyle = RollingFileAppender.RollingMode.Size,
                LockingModel = new FileAppender.MinimalLock()
            };
            rollingFileAppender.ActivateOptions();

            Hierarchy hierarchy = LogManager.GetRepository() as Hierarchy;
            Logger root = hierarchy.Root;
            root.RemoveAllAppenders();

            root.Level = Level.All;

            BasicConfigurator.Configure(rollingFileAppender);

        }
        
    }
}
