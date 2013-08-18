using System.Web.Http;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Services;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Model.Repository;
using OrangeJuice.Server.Web;

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

			container.RegisterType<IDateTimeProvider, UtcDateTimeProvider>(new ContainerControlledLifetimeManager());

			// Web
			container.RegisterType<AppKeyHandlerBase>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => new AppKeyHandlerFactory(c.Resolve<IEnvironmentProvider>()).Create()));
			container.RegisterType<IUrlEncoder, PercentUrlEncoder>(new ContainerControlledLifetimeManager());

			// UserController
			container.RegisterType<IUserRepository, EntityModelUserRepository>(new ContainerControlledLifetimeManager());

			// FoodController
			container.RegisterType<AwsOptions>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => new AswOptionsFactory(c.Resolve<IConfigurationProvider>()).Create()));
			container.RegisterType<AwsClientFactory>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<AwsOptions>(), container.Resolve<IUrlEncoder>(), container.Resolve<IDateTimeProvider>()));
			container.RegisterType<GroceryDescriptionFactory>(new ContainerControlledLifetimeManager());
		}
	}
}