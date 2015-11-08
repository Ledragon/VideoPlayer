using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace PlaylistModule
{
    public class PlayListModule : ModuleBase
    {
        public PlayListModule(IUnityContainer container, IRegionManager regionManager) : base(container, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<IPlayListViewModel, PlayListViewModel>();
            this.RegisterType<IPlayListView, PlayListView>();

            this.ReferenceRegion<IPlayListViewModel>(RegionNames.PlayListRegion);
        }
    }
}