using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public class HomeModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;

        public HomeModule(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this._regionManager = regionManager;
            this._unityContainer = unityContainer;
        }

        public override void Initialize()
        {
            this.RegisterType<IHomePageViewModel, HomePageViewModel>();
            this._unityContainer.RegisterType<Object, HomePage>(typeof(HomePage).FullName);
            this._regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof (HomePage));
        }
    }
}