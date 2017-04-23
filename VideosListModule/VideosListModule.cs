using Microsoft.Practices.Prism.Modularity;
using VideoPlayer.Infrastructure;
using VideosListModule.ViewModels;
using VideosListModule.Views;
using IModuleManager = VideoPlayer.Infrastructure.IModuleManager;

namespace VideosListModule
{
    public class VideosListModule : IModule
    {
        private readonly IModuleManager _moduleManager;
        //private readonly IRegionManager _regionManager;

        public VideosListModule(IModuleManager moduleManager)
            //: base(unityContainer, regionManager)
        {
            this._moduleManager = moduleManager;
            //this._regionManager = regionManager;
        }

        public void Initialize()
        {
            this._moduleManager
                .RegisterType<IVideoInfoView, VideoInfo>()
                .RegisterType<IVideoInfoViewModel, VideoInfoViewModel>()
                .RegisterType<IVideosListView, VideosListView>()
                .RegisterType<IVideosListViewModel, VideosListViewModel>()
                .RegisterViewWithRegion<IVideosListView>(RegionNames.VideosListRegion)
                .RegisterViewWithRegion<IVideoInfoView>(RegionNames.VideoInfoRegion);
            //this._regionManager.RegisterViewWithRegion(RegionNames.VideosListRegion, typeof (IVideosListView));
            //this._regionManager.RegisterViewWithRegion(RegionNames.VideoInfoRegion, typeof (IVideoInfoView));
        }
    }
}