using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace PlaylistModule
{
    public class PlayListModule : ModuleBase
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public PlayListModule(IUnityContainer container, IRegionManager regionManager) : base(container, regionManager)
        {
            this._container = container;
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            this.RegisterType<IPlayListViewModel, PlayListViewModel>();
            this.RegisterType<IPlayListView, PlayListView>();
            var regionName = RegionNames.PlayListRegion;
            var view = this._container.Resolve<IPlayListView>();
            if (this._regionManager.Regions.ContainsRegionWithName(regionName))
            {
                var region = this._regionManager.Regions[regionName];
                region.Add(view);
            }
            else
            {
                this._regionManager.RegisterViewWithRegion(regionName, view.GetType);
            }
            //this.ReferenceRegion<IPlayListViewModel>(RegionNames.PlayListRegion);
        }
    }
}