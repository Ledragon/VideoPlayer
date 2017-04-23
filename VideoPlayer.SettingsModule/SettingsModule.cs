using Microsoft.Practices.Prism.Modularity;
using VideoPlayer.Infrastructure;
using IModuleManager = VideoPlayer.Infrastructure.IModuleManager;

namespace VideoPlayer.SettingsModule
{
    public class SettingsModule : IModule
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