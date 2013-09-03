using System.Web.Http;
using System.Web.Http.Validation;

using FluentValidation;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Api.Validation.Infrustructure;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Model.Repository;
using OrangeJuice.Server.Filters;
using OrangeJuice.Server.Services;
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

			// Web API
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

			container.RegisterType<IAssemblyProvider, ReflectionAssemblyProvider>(new ContainerControlledLifetimeManager());

			// Web
			container.RegisterType<AppKeyHandlerBase>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => new AppKeyHandlerFactory(c.Resolve<IEnvironmentProvider>()).Create()));

			container.RegisterType<IUrlEncoder, PercentUrlEncoder>(new ContainerControlledLifetimeManager());

			// Validation
			container.RegisterType<IValidatorFactory, FluentValidation.Attributes.AttributedValidatorFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<IModelValidatorFactory, FluentModelValidatorFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<ModelValidatorProvider, FluentModelValidatorProvider>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<IValidatorFactory>(), container.Resolve<IModelValidatorFactory>()));

			// HomeController
			container.RegisterType<IApiInfoFactory, ApiInfoFactory>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<IAssemblyProvider>(), container.Resolve<IEnvironmentProvider>()));

			// UserController
			container.RegisterType<IUserRepository, EntityModelUserRepository>(new ContainerControlledLifetimeManager());

			// FoodController
			container.RegisterType<AwsOptions>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => new AswOptionsFactory(c.Resolve<IConfigurationProvider>()).Create()));

			container.RegisterType<IAwsClientFactory, AwsClientFactory>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<AwsOptions>(), container.Resolve<IUrlEncoder>(), container.Resolve<IDateTimeProvider>()));

			container.RegisterType<IAwsClient>(
				new PerResolveLifetimeManager(), // important!
				new InjectionFactory(c => c.Resolve<IAwsClientFactory>().Create()));

			container.RegisterType<IAwsProvider, XmlAwsProvider>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<IAwsClient>()));

			container.RegisterType<IFoodDescriptionFactory, XmlFoodDescriptionFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<IFilter<FoodDescription>, ValidImageFoodDescriptionFilter>(new ContainerControlledLifetimeManager());

			container.RegisterType<IFoodRepository, AwsFoodRepository>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<IAwsProvider>(), container.Resolve<IFoodDescriptionFactory>(), container.Resolve<IFilter<FoodDescription>>()));
		}
	}
}