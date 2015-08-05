using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace HomeModule
{
    public class VideoPlayerModule : ModuleBase
    {
        private readonly IUnityContainer _unityContainer;

        public VideoPlayerModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
            this._unityContainer = unityContainer;
        }

        public override void Initalize()
        {
            this._unityContainer.RegisterType<IVideoPlayerViewModel, VideoPlayerViewModel>();
        }
    }
}