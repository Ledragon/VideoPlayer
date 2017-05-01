using VideoPlayer.Infrastructure;
using VideoPlayer.ViewModels;

namespace VideoPlayer
{
    public class VideoPlayerModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;

        public VideoPlayerModule(IModuleManager moduleManager)
        {
            this._moduleManager = moduleManager;
        }

        public void Initialize()
        {
            this._moduleManager.RegisterType<IVideoPlayerViewModel, VideoPlayerViewModel>();
        }
    }
}