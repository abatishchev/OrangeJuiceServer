using System.Web.Http;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Model.Repository;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
    public static class UnityConfig
    {
		public static void InitializeContainer()
        {
            var container = BuildUnityContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IUserRepository, EntityModelUserRepository>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}