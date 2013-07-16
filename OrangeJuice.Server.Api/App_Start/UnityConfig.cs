using System.Web.Http;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api
{
    public static class UnityConfig
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            container.RegisterInstance<IModelValidator>(new ModelValidator());

            return container;
        }
    }
}