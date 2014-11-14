using System.IO;
using System.Windows;
using Log;
using VideoPlayer.Helpers;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            new LoggingSystemManager().SetPath(Path.Combine(FileSystemHelper.GetDefaultFolder(), "VideoPlayer.log"));
            Bootstrapper.Bootstrapper.BuildContainer();
        }
    }
}