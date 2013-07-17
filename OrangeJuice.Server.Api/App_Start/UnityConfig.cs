using System.Web.Http;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api
{
    public static class UnityConfig
    {
        public static void Initialize()
        {
            var container = BuildUnityContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterType<IUserRepository, UserRepositoryStub>(new ContainerControlledLifetimeManager());

            return container;
        }
    }
}