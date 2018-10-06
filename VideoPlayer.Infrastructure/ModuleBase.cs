using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using VideoPlayer.Infrastructure.ViewFirst;

namespace VideoPlayer.Infrastructure
{
    public abstract class ModuleBase : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;

        protected ModuleBase(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this._unityContainer = unityContainer;
            this._regionManager = regionManager;
        }

        public abstract void Initialize();

        public ModuleBase RegisterType<T, U>() where U : T
        {
            this._unityContainer.RegisterType<T, U>();
            return this;
        }

        public ModuleBase RegisterViewWithRegion<T>(String regionName) where T : IView
        {
            this._regionManager.RegisterViewWithRegion(regionName, typeof (T));
            return this;
        }

        public ModuleBase RegisterView<T>()
        {
            this._unityContainer.RegisterType<Object, T>(typeof (T).FullName);
            return this;
        }
    }
}