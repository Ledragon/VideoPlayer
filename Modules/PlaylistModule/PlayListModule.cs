using VideoPlayer.Infrastructure;

namespace PlaylistModule
{
    public class PlayListModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;
        //private readonly IRegionManager _regionManager;

        public PlayListModule(IModuleManager moduleManager) 
        {
            this._moduleManager = moduleManager;
            //this._regionManager = regionManager;
        }

        public void Initialize()
        {
            this._moduleManager.RegisterType<IPlayListViewModel, PlayListViewModel>()
                .RegisterType<IPlayListView, PlayListView>()
                .RegisterViewWithRegion<IPlayListView>(RegionNames.PlayListRegion);
            //this._regionManager.RegisterViewWithRegion(RegionNames.PlayListRegion, typeof (IPlayListView));
        }
    }
}