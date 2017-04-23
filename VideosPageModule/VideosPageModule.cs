using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace VideosPageModule
{
    public class VideosPageModule : ModuleBase
    {
        public VideosPageModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
        }

        public override void Initialize()
        {
            this.RegisterType<IVideosPageViewModel, VideosPageViewModel>()
                .RegisterView<VideosPage>()
                .RegisterType<IVideosPageButtonViewModel, VideosPageButtonViewModel>()
                .RegisterType<IVideosPageButtonView, VideosPageButtonView>()
                .RegisterViewWithRegion<IVideosPageButtonView>(RegionNames.NavigationRegion);
        }
    }
}