using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace VideoPlayer.Infrastructure
{
    public abstract class ModuleBase : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _unityContainer;

        public ModuleBase(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this._unityContainer = unityContainer;
            this._regionManager = regionManager;
        }

        public abstract void Initialize();

        protected void ReferenceRegion<T>(String regionName) where T : IViewModel
        {
            var viewModel = this._unityContainer.Resolve<T>();
            if (this._regionManager.Regions.ContainsRegionWithName(regionName))
            {
                var region = this._regionManager.Regions[regionName];
                region.Add(viewModel.View);
            }
            else
            {
                this._regionManager.RegisterViewWithRegion(regionName, viewModel.View.GetType());
            }
        }

        protected void RegisterType<T>()
        {
            this._unityContainer.RegisterType<T>();
        }

        protected IUnityContainer RegisterView<T>()
        {
            return this._unityContainer.RegisterType<Object, T>(typeof(T).FullName);
        }

        protected IUnityContainer RegisterType<T, U>() where U : T
        {
            return this._unityContainer.RegisterType<T, U>();
        }
    }
}