using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;

using FluentValidation;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Api.Models;
using OrangeJuice.Server.Api.Validation;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Model.Repository;
using OrangeJuice.Server.Filters;
using OrangeJuice.Server.Services;
using OrangeJuice.Server.Web;

using Unity.WebApi;

using ModelValidatorProvider = System.Web.Http.Validation.ModelValidatorProvider;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	internal static class UnityConfig
	{
		public static IUnityContainer InitializeContainer()
		{
			IUnityContainer container = new UnityContainer();

			RegisterTypes(container);

			// MVC
			// TODO: install resolver using Unity.Mvc4

			// Web API
			GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

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

			// Validation
			container.RegisterType<IValidatorFactory, ValidatorFactory>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container));

			container.RegisterType<IModelValidatorFactory, ModelValidatorFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<ModelValidatorProvider, FluentModelValidatorProvider>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<IValidatorFactory>(), container.Resolve<IModelValidatorFactory>()));

			container.RegisterType<IValidator<FoodSearchCriteria>, FoodSearchCriteriaValidator>(new ContainerControlledLifetimeManager())
					 .RegisterType<IValidator<UserRegistration>, UserRegistrationValidator>(new ContainerControlledLifetimeManager())
					 .RegisterType<IValidator<UserSearchCriteria>, UserSearchCriteriaValidator>(new ContainerControlledLifetimeManager());

			container.RegisterType<IUrlEncoder, PercentUrlEncoder>(new ContainerControlledLifetimeManager());

			// HomeController
			container.RegisterType<IApiInfoFactory, ApiInfoFactory>(new ContainerControlledLifetimeManager());

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

			container.RegisterType<IFoodDescriptionFactory, XmlFoodDescriptionFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<IFilter<FoodDescription>, ValidImageFoodDescriptionFilter>(new ContainerControlledLifetimeManager());

			container.RegisterType<IFoodRepository, AwsFoodRepository>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<IAwsClient>(), container.Resolve<IFoodDescriptionFactory>(), container.Resolve<IFilter<FoodDescription>>()));
		}
	}
}