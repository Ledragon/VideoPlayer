using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Module.Interfaces;
using Module.Views;
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
            this._container.RegisterType<VideoInfo>();
            this._container.RegisterType<IVideosList, VideosList>();
            this._container.RegisterType<IVideosListViewModel, VideosListViewModel>();
            this._container.RegisterType<ICategoryListViewModel, CategoryListViewModel>();
            this._container.RegisterType<ICategoryListView, CategoryList>();
            this._container.RegisterType<IVideoFilterGrid, VideoFilterGrid>();
            this._container.RegisterType<IVideoFilterGridViewModel, VideoFilterGridViewModel>();
            this._container.RegisterType<ITagsListView, TagsList>();
            this._container.RegisterType<ITagsListViewModel, TagsListViewModel>();
            this._container.RegisterType<ISortGrid, SortGrid>();
            this._container.RegisterType<ISortGridViewModel, SortGridViewModel>();
            this._container.RegisterType<IEditView, EditVideo>();
            this._container.RegisterType<IEditVideoViewModel, EditVideoViewModel>();

            this.ReferenceRegion<IVideosListViewModel>(RegionNames.VideosListRegion);
            this.ReferenceRegion<ICategoryListViewModel>(RegionNames.CategoriesListRegion);
            this.ReferenceRegion<IVideoFilterGridViewModel>(RegionNames.FilterRegion);
            this.ReferenceRegion<ITagsListViewModel>(RegionNames.TagsRegion);
            this.ReferenceRegion<ISortGridViewModel>(RegionNames.SortGridRegion);
            this.ReferenceRegion<IEditVideoViewModel>(RegionNames.EditVideoInfoRegion);
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