using System;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;

namespace VideoPlayer.Infrastructure
{
    public abstract class ModuleBase
    {
        private readonly IUnityContainer _unityContainer;
        private readonly IRegionManager _regionManager;

        public ModuleBase(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this._unityContainer = unityContainer;
            this._regionManager = regionManager;
        }

        protected void ReferenceRegion<T>(String regionName) where T : IViewModel
        {
            if (this._regionManager.Regions.ContainsRegionWithName(regionName))
            {
                IRegion region = this._regionManager.Regions[regionName];
                var viewModel = this._unityContainer.Resolve<T>();
                region.Add(viewModel.View);
            }
        }

        public virtual void Initalize()
        {

        }
    }
}