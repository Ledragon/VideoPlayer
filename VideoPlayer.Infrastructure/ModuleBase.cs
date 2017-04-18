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

        public virtual void Initialize()
        {
            throw new NotImplementedException();
        }

        protected void ReferenceRegion<T>(String regionName) where T : IViewModel
        {
            if (this._regionManager.Regions.ContainsRegionWithName(regionName))
            {
                var region = this._regionManager.Regions[regionName];
                var viewModel = this._unityContainer.Resolve<T>();
                region.Add(viewModel.View);
            }
        }

        protected void RegisterType<T>()
        {
            this._unityContainer.RegisterType<T>();
        }

        protected IUnityContainer RegisterType<T, U>() where U : T
        {
            return this._unityContainer.RegisterType<T, U>();
        }
    }
}