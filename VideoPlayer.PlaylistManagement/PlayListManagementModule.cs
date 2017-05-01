using VideoPlayer.Infrastructure;

namespace VideoPlayer.PlaylistManagement
{
    public class PlayListManagementModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;

        public PlayListManagementModule(IModuleManager moduleManager)
        {
            this._moduleManager = moduleManager;
        }

        public void Initialize()
        {
            this._moduleManager
                .RegisterType<IPlayListManagementViewModel, PlayListManagementViewModel>()
                .RegisterView<PlayListManagementView>()
                .RegisterType<IVideosPageButtonViewModel, VideosPageButtonViewModel>()
                .RegisterType<IVideosPageButtonView, VideosPageButtonView>()
                .RegisterViewWithRegion<IVideosPageButtonView>(RegionNames.NavigationRegion); 
        }
    }
}