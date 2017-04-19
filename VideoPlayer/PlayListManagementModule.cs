using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;
using VideoPlayer.ViewModels;
using VideoPlayer.Views;

namespace VideoPlayer
{
    public class PlayListManagementModule : ModuleBase
    {
        public PlayListManagementModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<IPlayListManagementView, PlayListManagementView>();
            this.RegisterType<IPlayListManagementViewModel, PlayListManagementViewModel>();
            this.ReferenceRegion<IPlayListManagementViewModel>(RegionNames.PlayListManagementRegion);

            this.RegisterType<IVideosListInteractionView, VideosListInteractionView>();
            this.RegisterType<IVideosListInteractionViewModel, VideosListInteractionViewModel>();
            this.ReferenceRegion<IVideosListInteractionViewModel>(RegionNames.VideosListInteraction);
        }
    }
}