using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace VideoPlayer.SettingsModule
{
    public class SettingsModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public SettingsModule(IUnityContainer unityContainer, IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            this.RegisterType<ISettingsViewModel, SettingsViewModel>()
                .RegisterType<ISettingsButtonViewModel, SettingsButtonViewModel>()
                .RegisterType<ISettingsPageButtonView, SettingsPageButtonView>();
            this.RegisterView<SettingsPage>();
            this._regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof (ISettingsPageButtonView));
        }
    }
}