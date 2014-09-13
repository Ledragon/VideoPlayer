using Microsoft.Practices.Unity;
using VideoPlayer.Database.Repository;

namespace VideoPlayer.Bootstrapper
{
    public static class Bootstrapper
    {
        public static void BuildContainer()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            Locator.Container = container;
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IVideoRepository, FileVideoRepository>();
        }
    }
}
