using System.Web.Http;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Model.Repository;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	internal static class UnityConfig
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
			container.RegisterType<IConfigurationProvider, AppSettingsConfigurationProvider>(new ContainerControlledLifetimeManager());

			container.RegisterType<IEnvironmentProvider, ConfigurationEnvironmentProvider>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<IConfigurationProvider>()));

			// Web
			container.RegisterType<AppKeyHandlerBase>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => new AppKeyHandlerFactory(c.Resolve<IEnvironmentProvider>()).Create()));
			container.RegisterType<IUserRepository, EntityModelUserRepository>(new ContainerControlledLifetimeManager());
		}
	}
}