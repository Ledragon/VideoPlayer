using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace VlcPlayer
{
    public class VlcPlayerModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public VlcPlayerModule(IUnityContainer container, IRegionManager regionManager) : base(container, regionManager)
        {
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            this.RegisterType<IPlayerViewModel, PlayerViewModel>();
            //this.RegisterView<Player>();
            this.RegisterType<IPlayer, Player>();
            this._regionManager.RegisterViewWithRegion(RegionNames.PlayerRegion, typeof(IPlayer));
            //this.ReferenceRegion<IPlayerViewModel>(RegionNames.PlayerRegion);
        }
    }
}