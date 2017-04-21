using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace VideosPageModule
{
    public class VideosPageModule : ModuleBase
    {
        private readonly IRegionManager _regionManager;

        public VideosPageModule(IUnityContainer unityContainer, IRegionManager regionManager)
            : base(unityContainer, regionManager)
        {
            this._regionManager = regionManager;
        }

        public override void Initialize()
        {
            this.RegisterType<IVideosPageViewModel, VideosPageViewModel>();
            this.RegisterView<VideosPage>();

            this.RegisterType<IVideosPageButtonViewModel, VideosPageButtonViewModel>()
                .RegisterType<IVideosPageButtonView, VideosPageButtonView>();
            this._regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(IVideosPageButtonView));
        }
    }
}