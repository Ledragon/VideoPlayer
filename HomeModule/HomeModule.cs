using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public class HomeModule : ModuleBase
    {
        private readonly IUnityContainer _unityContainer;

        public HomeModule(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this._unityContainer = unityContainer;
        }

        public override void Initialize()
        {
            this.RegisterType<IHomePageViewModel, HomePageViewModel>();
            this.RegisterType<IHomePage, HomePage>();
            //this._unityContainer.RegisterType<Object, HomePage>(typeof(HomePage).FullName);
            this.ReferenceRegion<IHomePageViewModel>(RegionNames.ContentRegion);
        }
    }
}