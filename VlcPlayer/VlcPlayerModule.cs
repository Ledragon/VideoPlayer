using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure;

namespace VlcPlayer
{
    public class VlcPlayerModule : IPrismModule
    {
        private readonly IModuleManager _moduleManager;

        public VlcPlayerModule(IModuleManager moduleManager)
        {
            this._moduleManager = moduleManager;
        }

        public void Initialize()
        {
            this._moduleManager.RegisterType<IPlayerViewModel, PlayerViewModel>()
            .RegisterType<IPlayer, Player>()
                //.RegisterType<IPlayer, Player>()
                .RegisterView<Player>();
            this._moduleManager.RegisterViewWithRegion<IPlayer>(RegionNames.PlayerRegion);
        }
    }
}