using VideoPlayer.Infrastructure;

namespace VideoPlayer.PlaylistManagement
{
    public class PlayListManagementModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;
        //private readonly IRegionManager _regionManager;

        public PlayListManagementModule(IModuleManager moduleManager)
            //: base(unityContainer, regionManager)
        {
            this._moduleManager = moduleManager;
            //this._regionManager = regionManager;
        }

        public void Initialize()
        {
            this._moduleManager
                .RegisterType<IPlayListManagementViewModel, PlayListManagementViewModel>()
                .RegisterView<PlayListManagementView>()
                .RegisterType<IVideosPageButtonViewModel, VideosPageButtonViewModel>()
                .RegisterType<IVideosPageButtonView, VideosPageButtonView>()
                .RegisterViewWithRegion<IVideosPageButtonView>(RegionNames.NavigationRegion); ;
            //this.RegisterType<IPlayListManagementViewModel, PlayListManagementViewModel>();
            //this._regionManager.RegisterViewWithRegion(RegionNames.PlayListManagementRegion,
            //    typeof (IPlayListManagementView));
        }
    }
}