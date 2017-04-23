using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.SettingsModule
{
    public class SettingsModule : ModuleBase
    {

        public SettingsModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<ISettingsViewModel, SettingsViewModel>()
                .RegisterType<ISettingsButtonViewModel, SettingsButtonViewModel>()
                .RegisterType<ISettingsPageButtonView, SettingsPageButtonView>()
                .RegisterView<SettingsPage>()
                .RegisterViewWithRegion<ISettingsPageButtonView>(RegionNames.NavigationRegion);
        }
    }
}