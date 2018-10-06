using VideoPlayer.Infrastructure;

namespace VideoPlayer.SettingsModule
{
    public class SettingsModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;

        public SettingsModule(IModuleManager moduleManager)
        {
            this._moduleManager = moduleManager;
        }

        public void Initialize()
        {
            this._moduleManager
                .RegisterType<ISettingsViewModel, SettingsViewModel>()
                .RegisterType<ISettingsButtonViewModel, SettingsButtonViewModel>()
                .RegisterType<ISettingsPageButtonView, SettingsPageButtonView>()
                .RegisterView<SettingsPage>()
                .RegisterViewWithRegion<ISettingsPageButtonView>(RegionNames.NavigationRegion);
        }
    }
}