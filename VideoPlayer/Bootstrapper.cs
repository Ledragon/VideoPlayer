using System.Windows;
using HomeModule;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Module;
using PlaylistModule;
using VideoPlayer.Common;
using VideoPlayer.Database.Repository;
using VideoPlayer.Services;
using VideoPlayer.ViewModels;
using VlcPlayer;

namespace VideoPlayer
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            this.Container.RegisterType<IVideoRepository, FileVideoRepository>();
            this.Container.RegisterType<ILibraryService, LibraryService>();
            this.Container.RegisterType<VideosTabControlViewModel>();

            //TEMP
            this.Container.RegisterType<IVideoPlayerViewModel, VideoPlayerViewModel>();
            this.Container.RegisterType<IVideoPlayer, HomeModule.VideoPlayer>();
            Locator.Container = this.Container;
            return this.Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window) this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            this.AddModule<ModuleModule>();
            this.AddModule<PlayListModule>();
            this.AddModule<VlcPlayerModule>();
            this.AddModule<HomeModule.HomeModule>();
            this.AddModule<PlayListManagementModule>();
        }

        private void AddModule<T>()
        {
            var moduleType = typeof (T);
            var moduleInfo = new ModuleInfo
            {
                ModuleName = moduleType.Name,
                ModuleType = moduleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            };
            this.ModuleCatalog.AddModule(moduleInfo);
        }
    }
}