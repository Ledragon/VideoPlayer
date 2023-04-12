using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using LeDragon.Log.Standard;
using VideoPlayer.Helpers;

namespace VideoPlayer
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            try
            {
                Current.Startup +=
                    (sender, e) =>
                    {
                        SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
                    };
                Current.Exit += (sender, e) => { SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS); };
            }
            catch (Exception e)
            {
                this.Logger().Error(e);
            }
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                LoggingSystemManager.SetPath(Path.Combine(FileSystemHelper.GetDefaultFolder(), "VideoPlayer.log"));
                var bootstrapper = new Bootstrapper();
                bootstrapper.Run();
            }
            catch (Exception ex)
            {
                this.Logger().Error(ex);
            }
        }

        //    Bootstrapper.Bootstrapper.BuildContainer();
        //{

        //private void Application_Startup(object sender, StartupEventArgs e)
        //}
    }


    [Flags]
    public enum EXECUTION_STATE : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001
        // Legacy flag, should not be used.
        // ES_USER_PRESENT = 0x00000004
    }
}