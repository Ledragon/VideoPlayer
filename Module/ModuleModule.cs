﻿using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Module.Interfaces;
using Module.Views;
using VideoPlayer.Infrastructure;

namespace Module
{
    public class ModuleModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public ModuleModule(IUnityContainer container, IRegionManager regionManager) : base(container, regionManager)
        {
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            //this.RegisterType<IVideosListView, VideosListView>();
            this.RegisterType<ICategoryListViewModel, CategoryListViewModel>();
            this.RegisterType<ICategoryListView, CategoryList>();
            this._regionManager.RegisterViewWithRegion(RegionNames.CategoriesListRegion, typeof (ICategoryListView));

            this.RegisterType<IVideoFilterGrid, VideoFilterGrid>();
            this.RegisterType<IVideoFilterGridViewModel, VideoFilterGridViewModel>();
            this._regionManager.RegisterViewWithRegion(RegionNames.FilterRegion, typeof (IVideoFilterGrid));
            //this.ReferenceRegion<IVideoFilterGridViewModel>(RegionNames.FilterRegion);

            this.RegisterType<ITagsListView, TagsList>();
            this.RegisterType<ITagsListViewModel, TagsListViewModel>();

            this.RegisterType<ISortGrid, SortGrid>();
            this.RegisterType<ISortGridViewModel, SortGridViewModel>();

            this.RegisterType<IEditView, EditVideo>();
            this.RegisterType<IEditVideoViewModel, EditVideoViewModel>();

            //this.ReferenceRegion<ICategoryListViewModel>(RegionNames.CategoriesListRegion);
            this.ReferenceRegion<ITagsListViewModel>(RegionNames.TagsRegion);
            this.ReferenceRegion<ISortGridViewModel>(RegionNames.SortGridRegion);
            this.ReferenceRegion<IEditVideoViewModel>(RegionNames.EditVideoInfoRegion);
        }
    }
}