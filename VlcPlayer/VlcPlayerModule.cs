using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace VlcPlayer
{
    public class VlcPlayerModule : ModuleBase
    {
        public VlcPlayerModule(IUnityContainer container, IRegionManager regionManager) : base(container, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<IPlayerViewModel, PlayerViewModel>();
            this.RegisterType<IPlayer, Player>();
            this.ReferenceRegion<IPlayerViewModel>(RegionNames.PlayerRegion);
        }
    }
}