using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;
using VideosListModule.ViewModels;
using VideosListModule.Views;

namespace VideosListModule
{
    public class VideosListModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public VideosListModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            this.RegisterType<IVideoInfoView, VideoInfo>()
                .RegisterType<IVideoInfoViewModel, VideoInfoViewModel>()
                .RegisterType<IVideosListView, VideosListView>()
                .RegisterType<IVideosListViewModel, VideosListViewModel>()
                .RegisterType<VideosPageButtonViewModel>()
                .RegisterType<VideosPageButton>();
            this._regionManager.RegisterViewWithRegion(RegionNames.VideosListRegion, typeof (IVideosListView));
            this._regionManager.RegisterViewWithRegion(RegionNames.VideoInfoRegion, typeof (IVideoInfoView));
            this._regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(VideosPageButton));
        }
    }
}