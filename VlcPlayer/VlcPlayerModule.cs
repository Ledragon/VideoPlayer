using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace VlcPlayer
{
    public class VlcPlayerModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public VlcPlayerModule(IUnityContainer container, IRegionManager regionManager)
        {
            this._container = container;
            this._regionManager = regionManager;
        }

        public void Initialize()
        {
            this._container.RegisterType<IPlayerViewModel, PlayerViewModel>();
            this._container.RegisterType<IPlayer, Player>();
            this.ReferenceRegion<IPlayerViewModel>(RegionNames.PlayerRegion);
        }

        private void ReferenceRegion<T>(string regionName) where T : IViewModel
        {
            if (this._regionManager.Regions.ContainsRegionWithName(regionName))
            {
                var region = this._regionManager.Regions[regionName];
                var viewModel = this._container.Resolve<T>();
                region.Add(viewModel.View);
            }
        }
    }
}