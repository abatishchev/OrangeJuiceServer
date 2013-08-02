using System.Web.Http;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Model.Repository;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	static class UnityConfig
	{
		public static IUnityContainer InitializeContainer()
		{
			IUnityContainer container = new UnityContainer();

			RegisterTypes(container);

			GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

			return container;
		}

		private static void RegisterTypes(IUnityContainer container)
		{
			// Providers
			IConfigurationProvider configurationProvider = new AppSettingsConfigurationProvider();

			container.RegisterInstance(configurationProvider);
			container.RegisterType<IEnvironmentProvider, ConfigurationEnvironmentProvider>(new ContainerControlledLifetimeManager(), new InjectionConstructor(configurationProvider));

			// Web
			container.RegisterType<IUserRepository, EntityModelUserRepository>(new ContainerControlledLifetimeManager());
		}
	}
}