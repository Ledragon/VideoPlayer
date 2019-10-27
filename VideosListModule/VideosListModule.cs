using VideoPlayer.Infrastructure;
using VideosListModule.ViewModels;
using VideosListModule.Views;

namespace VideosListModule
{
    public class VideosListModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;

        public VideosListModule(IModuleManager moduleManager)
        {
            this._moduleManager = moduleManager;
        }

        public void Initialize()
        {
            this._moduleManager
                //.RegisterType<IVideoInfoView, VideoInfo>()
                .RegisterType<IVideoInfoView, HorizVideoInfo>()
                .RegisterType<IVideoInfoViewModel, VideoInfoViewModel>()
                //.RegisterType<IVideosListView, VideosListView>()
                .RegisterType<IVideosListView, HorizVideoMasterDetails>()
                .RegisterType<IVideosListViewModel, VideosListViewModel>()
                .RegisterViewWithRegion<IVideosListView>(RegionNames.VideosListRegion)
                .RegisterViewWithRegion<IVideoInfoView>(RegionNames.VideoInfoRegion);
        }
    }
}