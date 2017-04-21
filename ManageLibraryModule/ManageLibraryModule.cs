using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace ManageLibraryModule
{
    public class ManageLibraryModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public ManageLibraryModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            this.RegisterType<IEditViewModel, EditViewModel>()
                .RegisterType<IManagePageButtonViewModel, ManagePageButtonViewModel>()
                .RegisterType<IManagePageButtonView, ManagePageButtonView>();
            this.RegisterView<EditView>();
            this._regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(IManagePageButtonView));
        }
    }
}