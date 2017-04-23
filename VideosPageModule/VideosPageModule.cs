using VideoPlayer.Infrastructure;

namespace VideosPageModule
{
    public class VideosPageModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;

        public VideosPageModule(IModuleManager moduleManager)
        {
            this._moduleManager = moduleManager;
        }

        public void Initialize()
        {
            this._moduleManager
                .RegisterType<IVideosPageViewModel, VideosPageViewModel>()
                .RegisterView<VideosPage>()
                .RegisterType<IVideosPageButtonViewModel, VideosPageButtonViewModel>()
                .RegisterType<IVideosPageButtonView, VideosPageButtonView>()
                .RegisterViewWithRegion<IVideosPageButtonView>(RegionNames.NavigationRegion);
        }
    }
}