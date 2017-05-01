using HomeModule;
using VideoPlayer.Infrastructure;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    public class VideoPlayerModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;

        public VideoPlayerModule(IModuleManager moduleManager)
            //: base(unityContainer, regionManager)
        {
            this._moduleManager = moduleManager;
        }

        public void Initialize()
        {
            this._moduleManager.RegisterType<IVideoPlayerViewModel, VideoPlayerViewModel>();
        }
    }
}