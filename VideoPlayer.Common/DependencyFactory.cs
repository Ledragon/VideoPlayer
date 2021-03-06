﻿using Microsoft.Practices.Unity;

namespace VideoPlayer.Common
{
    public static class DependencyFactory
    {
        public static T Resolve<T>()
        {
            T instance = default(T);
            if (Locator.Container.IsRegistered<T>())
            {
                instance = Locator.Container.Resolve<T>();
            }
            return instance;
        }
    }
}