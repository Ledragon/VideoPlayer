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
            this.RegisterType<VideoInfo>();
            this.RegisterType<IVideosListView, VideosListView>();
            this.RegisterType<IVideosListViewModel, VideosListViewModel>();
            this.ReferenceRegion<IVideosListViewModel>(RegionNames.VideosListRegion);
        }
    }
}