using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public class HomeModule : ModuleBase
    {

        public HomeModule(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<IHomePageViewModel, HomePageViewModel>();
            this.RegisterType<IHomePage, HomePage>();
            this.ReferenceRegion<IHomePageViewModel>(RegionNames.HomeRegion);
        }
    }
}