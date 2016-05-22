using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;
using VideosListModule.Views;

namespace VideosListModule
{
    public class VideosListModule : ModuleBase
    {
        public VideosListModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<IVideoInfoView, VideoInfo>();
            this.RegisterType<IVideoInfoViewModel, VideoInfoViewModel>();
            this.RegisterType<IVideosListView, VideosListView>();
            this.RegisterType<IVideosListViewModel, VideosListViewModel>();
            this.ReferenceRegion<IVideosListViewModel>(RegionNames.VideosListRegion);
            this.ReferenceRegion<IVideoInfoViewModel>(RegionNames.VideoInfoRegion);
        }
    }
}