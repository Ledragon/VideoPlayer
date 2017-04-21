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
            this.Container.RegisterType<IVideoRepository, FileVideoRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ILibraryService, LibraryService>(new ContainerControlledLifetimeManager())
                .RegisterType<ICategoryService, CategoryService>(new ContainerControlledLifetimeManager());
                //.RegisterType<IVideosPageViewModel, VideosPageViewModel>();

            //TEMP
            this.Container.RegisterType<IVideoPlayerViewModel, VideoPlayerViewModel>();
            this.Container.RegisterType<IVideoPlayer, HomeModule.VideoPlayer>();
            this.Container.RegisterType<IEditViewModel, EditViewModel>();
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
            this.AddModule<ModuleModule>()
                .AddModule<PlayListModule>()
                .AddModule<VlcPlayerModule>()
                .AddModule<HomeModule.HomeModule>()
                .AddModule<PlayListManagementModule>()
                .AddModule<VideosListModule.VideosListModule>()
                .AddModule<VideosPageModule.VideosPageModule>();
        }

        private Bootstrapper AddModule<T>()
        {
            var moduleType = typeof (T);
            var moduleInfo = new ModuleInfo
            {
                ModuleName = moduleType.Name,
                ModuleType = moduleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            };
            this.ModuleCatalog.AddModule(moduleInfo);
            return this;
        }
    }
}