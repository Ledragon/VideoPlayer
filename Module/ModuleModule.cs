using Microsoft.Practices.Prism.Regions;
using Module.Interfaces;
using Module.Views;
using Microsoft.Practices.Unity;
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
            this.RegisterType<ICategoryListViewModel, CategoryListViewModel>();
            this.RegisterType<ICategoryListView, CategoryList>();
            this._regionManager.RegisterViewWithRegion(RegionNames.CategoriesListRegion, typeof(ICategoryListView));

            this.RegisterType<IVideoFilterGrid, VideoFilterGrid>();
            this.RegisterType<IVideoFilterGridViewModel, VideoFilterGridViewModel>();
            this._regionManager.RegisterViewWithRegion(RegionNames.FilterRegion, typeof(IVideoFilterGrid));

            this.RegisterType<ITagsListView, TagsList>();
            this.RegisterType<ITagsListViewModel, TagsListViewModel>();
            this._regionManager.RegisterViewWithRegion(RegionNames.TagsRegion, typeof(ITagsListView));

            this.RegisterType<ISortGrid, SortGrid>();
            this.RegisterType<ISortGridViewModel, SortGridViewModel>();
            this._regionManager.RegisterViewWithRegion(RegionNames.SortGridRegion, typeof(ISortGrid));
            //this.ReferenceRegion<ISortGridViewModel>(RegionNames.SortGridRegion);

            this.RegisterType<IEditView, EditVideo>();
            this.RegisterType<IEditVideoViewModel, EditVideoViewModel>();
            this._regionManager.RegisterViewWithRegion(RegionNames.EditVideoInfoRegion, typeof(IEditView));
        }
    }
}