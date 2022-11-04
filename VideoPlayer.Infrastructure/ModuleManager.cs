using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.Infrastructure
{
    public class ModuleManager : IModuleManager
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IRegionManager _regionManager;

        public ModuleManager(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this._unityContainer = unityContainer;
            this._regionManager = regionManager;
        }

        public IModuleManager RegisterType<T, U>() where U : T
        {
            this._unityContainer.RegisterType<T, U>();
            return this;
        }

        public IModuleManager RegisterViewWithRegion<T>(String regionName) where T : IView
        {
            this._regionManager.RegisterViewWithRegion(regionName, typeof(T));
            return this;
        }

        public IModuleManager RegisterView<T>()
        {
            this._unityContainer.RegisterType<Object, T>(typeof(T).FullName);
            return this;
        }
    }
}