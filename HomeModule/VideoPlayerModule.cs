using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public class VideoPlayerModule : ModuleBase
    {
        public VideoPlayerModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<IVideoPlayerViewModel, VideoPlayerViewModel>();
        }
    }
}