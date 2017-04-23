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

        protected ModuleBase(IUnityContainer unityContainer, IRegionManager regionManager)
        {
            this._unityContainer = unityContainer;
            this._regionManager = regionManager;
        }

        public abstract void Initialize();

        protected void ReferenceRegion<T>(String regionName) where T : IViewModel
        {
            var viewModel = this._unityContainer.Resolve<T>();
            var view = viewModel.View;
            this.ReferenceRegion(regionName, view);
        }

        //protected void ReferenceRegionByView<T>(String regionName) where T : ViewFirst.IView
        //{
        //    this.GetValue<T>(regionName, view);
        //}

        private void ReferenceRegion(String regionName, IView view) //where T : IViewModel
        {
            if (this._regionManager.Regions.ContainsRegionWithName(regionName))
            {
                var region = this._regionManager.Regions[regionName];
                region.Add(view);
            }
            else
            {
                this._regionManager.RegisterViewWithRegion(regionName, view.GetType());
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