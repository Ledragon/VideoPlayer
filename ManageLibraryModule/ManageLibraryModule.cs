using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace ManageLibraryModule
{
    public class ManageLibraryModule : ModuleBase
    {
        public ManageLibraryModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<IEditViewModel, EditViewModel>();
            this.RegisterView<EditView>();
        }
    }
}