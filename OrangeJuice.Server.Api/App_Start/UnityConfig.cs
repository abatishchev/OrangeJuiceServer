using System;
using System.Web.Http;
using System.Web.Http.Validation;
using System.Xml.Linq;

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
				new InjectionConstructor(typeof(IConfigurationProvider)));

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
				new InjectionConstructor(typeof(IValidatorFactory), typeof(IModelValidatorFactory)));

			// VersionController
			container.RegisterType<IFactory<ApiVersion>, ApiVersionFactory>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IAssemblyProvider), typeof(IEnvironmentProvider)));

			container.RegisterType<ApiVersion>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => c.Resolve<IFactory<ApiVersion>>().Create()));

			// UserController
			container.RegisterType<IUserRepository, EntityModelUserRepository>(new ContainerControlledLifetimeManager());

			// FoodController
			container.RegisterType<AwsOptions>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => new AswOptionsFactory(c.Resolve<IConfigurationProvider>()).Create()));

			container.RegisterType<IArgumentBuilder, AwsArgumentBuilder>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<AwsOptions>().AccessKey, container.Resolve<AwsOptions>().AssociateTag, typeof(IDateTimeProvider)));

			container.RegisterType<IArgumentFormatter, FlattenArgumentFormatter>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IUrlEncoder)));

			container.RegisterType<IQuerySigner, AwsQuerySigner>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<AwsOptions>().SecretKey, typeof(IUrlEncoder)));

			container.RegisterType<IQueryBuilder, AwsQueryBuilder>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IArgumentBuilder), typeof(IArgumentFormatter), typeof(IQuerySigner)));

			container.RegisterType<IDocumentLoader, HttpDocumentLoader>(new TransientLifetimeManager());

			container.RegisterType<IValidator<XElement>, XmlItemValidator>(new ContainerControlledLifetimeManager());

			container.RegisterType<IItemSelector, XmlItemSelector>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IValidator<XElement>)));

			container.RegisterType<IAwsClient, AwsClient>(
				new TransientLifetimeManager(),
				new InjectionConstructor(typeof(IQueryBuilder), typeof(IDocumentLoader), typeof(IItemSelector)));

			container.RegisterType<IAwsProvider, AwsProvider>(
				new TransientLifetimeManager(),
				new InjectionConstructor(typeof(IAwsClient)));

			container.RegisterType<IFactory<IAwsProvider>, ProxyFactory<IAwsProvider>>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(new Func<IAwsProvider>(() => container.Resolve<IAwsProvider>())));

			container.RegisterType<IFoodDescriptionFactory, XmlFoodDescriptionFactory>(new ContainerControlledLifetimeManager());

			container.RegisterType<IFilter<FoodDescription>, ValidImageFoodDescriptionFilter>(new ContainerControlledLifetimeManager());

			container.RegisterType<IFoodRepository, AwsFoodRepository>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IFactory<IAwsProvider>), typeof(IFoodDescriptionFactory), typeof(IFilter<FoodDescription>)));
		}
	}
}