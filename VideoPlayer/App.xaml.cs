using System;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                new LoggingSystemManager().SetPath(Path.Combine(FileSystemHelper.GetDefaultFolder(), "VideoPlayer.log"));
                var bootstrapper = new Bootstrapper();
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                this.Logger().Error(ex);
            }
        }

        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    Bootstrapper.Bootstrapper.BuildContainer();
        //}
    }
}