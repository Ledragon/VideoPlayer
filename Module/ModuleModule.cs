using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Module.Interfaces;
using Module.Views;
using VideoPlayer.Infrastructure;

namespace Module
{
    public class ModuleModule : ModuleBase
    {
        public ModuleModule(IUnityContainer container, IRegionManager regionManager) : base(container, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<VideoInfo>();
            this.RegisterType<IVideosList, VideosList>();
            this.RegisterType<IVideosListViewModel, VideosListViewModel>();
            this.RegisterType<ICategoryListViewModel, CategoryListViewModel>();
            this.RegisterType<ICategoryListView, CategoryList>();
            this.RegisterType<IVideoFilterGrid, VideoFilterGrid>();
            this.RegisterType<IVideoFilterGridViewModel, VideoFilterGridViewModel>();
            this.RegisterType<ITagsListView, TagsList>();
            this.RegisterType<ITagsListViewModel, TagsListViewModel>();
            this.RegisterType<ISortGrid, SortGrid>();
            this.RegisterType<ISortGridViewModel, SortGridViewModel>();
            this.RegisterType<IEditView, EditVideo>();
            this.RegisterType<IEditVideoViewModel, EditVideoViewModel>();

            this.ReferenceRegion<IVideosListViewModel>(RegionNames.VideosListRegion);
            this.ReferenceRegion<ICategoryListViewModel>(RegionNames.CategoriesListRegion);
            this.ReferenceRegion<IVideoFilterGridViewModel>(RegionNames.FilterRegion);
            this.ReferenceRegion<ITagsListViewModel>(RegionNames.TagsRegion);
            this.ReferenceRegion<ISortGridViewModel>(RegionNames.SortGridRegion);
            this.ReferenceRegion<IEditVideoViewModel>(RegionNames.EditVideoInfoRegion);
        }
    }
}