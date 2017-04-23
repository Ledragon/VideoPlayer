using VideoPlayer.Infrastructure;

namespace ManageLibraryModule
{
    public class ManageLibraryModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;

        public ManageLibraryModule(IModuleManager moduleManager)
        {
            this._moduleManager = moduleManager;
        }

        public void Initialize()
        {
            this._moduleManager
                .RegisterType<IEditViewModel, EditViewModel>()
                .RegisterType<IManagePageButtonViewModel, ManagePageButtonViewModel>()
                .RegisterType<IManagePageButtonView, ManagePageButtonView>()
                .RegisterView<ManageLibraryView>()
                .RegisterViewWithRegion<IManagePageButtonView>(RegionNames.NavigationRegion);
        }
    }
}