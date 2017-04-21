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
            this.RegisterType<IVideosPageViewModel, VideosPageViewModel>();
            this.RegisterView<VideosPage>();
            //this.ReferenceRegion<IVideosPageViewModel>(RegionNames.VideosPageRegion);
        }
    }
}