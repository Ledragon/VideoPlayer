using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Module;
using PlaylistModule;
using VideoPlayer.Common;
using VideoPlayer.Database.Repository;
using VideoPlayer.Database.Repository.Contracts;
using VideoPlayer.Infrastructure;
using VideoPlayer.Nfo;
using VideoPlayer.PlaylistManagement;
using VideoPlayer.Services;
using VideoPlayer.VideoListInteraction;
using VideoPlayer.ViewModels;
using VlcPlayer;
using IModuleManager = VideoPlayer.Infrastructure.IModuleManager;

namespace VideoPlayer
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            this.Container.RegisterType<ILibraryRepository, FileLibraryRepository>(new ContainerControlledLifetimeManager())
                .RegisterType<ILibraryService, LibraryService>(new ContainerControlledLifetimeManager())
                .RegisterType<ICategoryService, CategoryService>(new ContainerControlledLifetimeManager())
                .RegisterType<IPlaylistService, PlaylistService>(new ContainerControlledLifetimeManager())
                .RegisterType<IModuleManager, Infrastructure.ModuleManager>(new ContainerControlledLifetimeManager())
                .RegisterType<INfoSerializer, NfoSerializer>(new ContainerControlledLifetimeManager())
                .RegisterType<INfoService, NfoService>(new ContainerControlledLifetimeManager())
                .RegisterType<ISettingsService, SettingsService>(new ContainerControlledLifetimeManager())
                .RegisterType<StackPanelRegionAdapter>();

            //TEMP
            this.Container.RegisterType<IVideoPlayerViewModel, VideoPlayerViewModel>();
            //this.Container.RegisterType<IVideoPlayer, HomeModule.VideoPlayer>();
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
                .AddModule<VideosListModule.VideosListModule>()
                .AddModule<HomeModule.HomeModule>()
                .AddModule<VideoListInteractionModule>()
                .AddModule<SettingsModule.SettingsModule>()
                .AddModule<PlayListManagementModule>()
                .AddModule<ManageLibraryModule.ManageLibraryModule>()
                ;
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();
            mappings.RegisterMapping(typeof(StackPanel), this.Container.Resolve<StackPanelRegionAdapter>());
            return mappings;
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