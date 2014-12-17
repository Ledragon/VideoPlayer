using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Module.Interfaces;
using VideoPlayer.Infrastructure;

namespace Module
{
    public class ModuleModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public ModuleModule(IUnityContainer container, IRegionManager regionManager)
        {
            this._container = container;
            this._regionManager = regionManager;
        }

        public void Initialize()
        {
            this._container.RegisterType<IHomeViewModel, HomeViewModel>();
            this._container.RegisterType<IHomeView, Home>();
            this._container.RegisterType<VideoInfo>();
            this._container.RegisterType<IVideosList, VideosList>();
            this._container.RegisterType<IVideosListViewModel, VideosListViewModel>();
            this._container.RegisterType<ICategoryListViewModel, CategoryListViewModel>();
            this._container.RegisterType<ICategoryListView, CategoryList>();
            this._container.RegisterType<IPlayerViewModel, PlayerViewModel>();
            this._container.RegisterType<IPlayer, Player>();
            this._container.RegisterType<IVideoFilterGrid, VideoFilterGrid>();
            this._container.RegisterType<IVideoFilterGridViewModel, VideoFilterGridViewModel>();

            //this.ReferenceRegion<IHomeView>(RegionNames.ContentRegion);
            this.ReferenceRegion<IVideosListViewModel>(RegionNames.VideosListRegion);
            this.ReferenceRegion<ICategoryListViewModel>(RegionNames.CategoriesListRegion);
            this.ReferenceRegion<IPlayerViewModel>(RegionNames.PlayerRegion);
            this.ReferenceRegion<IVideoFilterGridViewModel>(RegionNames.FilterRegion);
        }

        private void ReferenceRegion<T>(String regionName) where T:IViewModel
        {
            if (this._regionManager.Regions.ContainsRegionWithName(regionName))
            {
                IRegion region = this._regionManager.Regions[regionName];
                var viewModel = this._container.Resolve<T>();
                region.Add(viewModel.View);
            }
        }
    }
}