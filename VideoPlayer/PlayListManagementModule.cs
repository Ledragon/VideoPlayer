using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;
using VideoPlayer.ViewModels;
using VideoPlayer.Views;

namespace VideoPlayer
{
    public class PlayListManagementModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public PlayListManagementModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            this.RegisterType<IPlayListManagementView, PlayListManagementView>();
            this.RegisterType<IPlayListManagementViewModel, PlayListManagementViewModel>();
            this._regionManager.RegisterViewWithRegion(RegionNames.PlayListManagementRegion,
                typeof (IPlayListManagementView));
        }
    }
}