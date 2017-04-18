using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    public class VideosPageModule : ModuleBase
    {
        public VideosPageModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<IVideosPageViewModel, VideosPageViewModel>()
                .RegisterType<IVideosPageView, VideosPage>();
            this.ReferenceRegion<IVideosPageViewModel>(RegionNames.VideosPageRegion);
        }
    }
}