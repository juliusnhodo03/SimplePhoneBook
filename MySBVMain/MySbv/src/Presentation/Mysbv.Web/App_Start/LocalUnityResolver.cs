﻿using Unity;
using Unity.Lifetime;

namespace Mysbv.Web
{
    public static class LocalUnityResolver
    {
        private static readonly UnityContainer UnityContainer = new UnityContainer();
        public static void Register<I, T>() where T : I
        {
            UnityContainer.RegisterType<I, T>(new ContainerControlledLifetimeManager());
        }
        public static void InjectStub<I>(I instance)
        {
            UnityContainer.RegisterInstance(instance, new ContainerControlledLifetimeManager());
        }
        public static T Retrieve<T>()
        {
            return UnityContainer.Resolve<T>();
        }
    }
}