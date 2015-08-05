using System.Windows;
using HomeModule;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Module;
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
            //base.ConfigureModuleCatalog();
            var moduleType = typeof (ModuleModule);
            this.ModuleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = moduleType.Name,
                ModuleType = moduleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });

            var vlcPlayerModuleType = typeof (VlcPlayerModule);
            this.ModuleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = vlcPlayerModuleType.Name,
                ModuleType = vlcPlayerModuleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });

            var homeModuleType = typeof (HomeModule.HomeModule);
            this.ModuleCatalog.AddModule(new ModuleInfo
            {
                ModuleName = homeModuleType.Name,
                ModuleType = homeModuleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });
        }
    }
}