using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public class HomeModule : IModule
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IRegionManager _regionManager;

        public HomeModule(IUnityContainer unityContainer, IRegionManager regionManager) //: base(unityContainer, regionManager)
        {
            this._unityContainer = unityContainer;
            this._regionManager = regionManager;
        }

        public void Initialize()
        {

            this._unityContainer.RegisterType<IHomePageViewModel, HomePageViewModel>();
            this._unityContainer.RegisterType<IHomePage, HomePage>();


            if (this._regionManager.Regions.ContainsRegionWithName(RegionNames.HomeRegion))
            {
                IRegion region = this._regionManager.Regions[RegionNames.HomeRegion];
                var viewModel = this._unityContainer.Resolve<IHomePageViewModel>();
                region.Add(viewModel.View);
            }
        }
    }
}