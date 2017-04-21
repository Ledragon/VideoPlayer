using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace PlaylistModule
{
    public class PlayListModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public PlayListModule(IUnityContainer container, IRegionManager regionManager) : base(container, regionManager)
        {
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            this.RegisterType<IPlayListViewModel, PlayListViewModel>();
            this.RegisterType<IPlayListView, PlayListView>();
            this._regionManager.RegisterViewWithRegion(RegionNames.PlayListRegion, typeof (IPlayListView));
        }
    }
}