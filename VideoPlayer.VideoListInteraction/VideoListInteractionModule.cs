using Microsoft.Practices.Prism.Regions;
using Unity;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.VideoListInteraction
{
    public class VideoListInteractionModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public VideoListInteractionModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            this.RegisterType<IVideosListInteractionView, VideosListInteractionView>();
            this.RegisterType<IVideosListInteractionViewModel, VideosListInteractionViewModel>();
            this._regionManager.RegisterViewWithRegion(RegionNames.VideosListInteraction,
                typeof (IVideosListInteractionView));
        }
    }
}