using System.Web.Http;

using Microsoft.Practices.Unity;

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

            return container;
        }
    }
}