using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.Infrastructure
{
    public interface IModuleManager
    {
        IModuleManager RegisterType<T, U>() where U : T;
        IModuleManager RegisterView<T>();
        IModuleManager RegisterViewWithRegion<T>(System.String regionName) where T : IView;
    }
}