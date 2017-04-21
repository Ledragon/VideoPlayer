using System;
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
            this._container.RegisterType<Object, PlayListView>(typeof(PlayListView).FullName);
            this._regionManager.RegisterViewWithRegion(RegionNames.PlayListRegion, typeof (IPlayListView));
        }
    }
}