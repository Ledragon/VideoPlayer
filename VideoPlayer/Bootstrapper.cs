using System;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using VideoPlayer.Common;
using VideoPlayer.Database.Repository;
using VideoPlayer.Services;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    public class Bootstrapper:UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            this.Container.RegisterType<IVideoRepository, FileVideoRepository>();
            this.Container.RegisterType<ILibraryService, LibraryService>();
            this.Container.RegisterType<VideosTabControlViewModel>();
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
            Type moduleType = typeof (Module.ModuleModule);
            this.ModuleCatalog.AddModule(new ModuleInfo()
            {
                ModuleName=moduleType.Name,
                ModuleType = moduleType.AssemblyQualifiedName,
                InitializationMode = InitializationMode.WhenAvailable
            });
        }
    }
}