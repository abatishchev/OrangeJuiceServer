using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.Validation;
using System.Xml.Linq;

using FluentValidation;
using FluentValidation.Attributes;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Api.Policies;
using OrangeJuice.Server.Api.Validation.Infrustructure;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Container;
using OrangeJuice.Server.Data.Repository;
using OrangeJuice.Server.Data.Unit;
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
			#region Providers
			container.RegisterType<IConfigurationProvider, AppSettingsConfigurationProvider>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IEnvironmentProvider, ConfigurationEnvironmentProvider>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IConfigurationProvider)));

			container.RegisterType<IDateTimeProvider, UtcDateTimeProvider>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IAssemblyProvider, ReflectionAssemblyProvider>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IErrorDetailPolicyProvider, EnvironmentErrorDetailPolicyProvider>(
				new HierarchicalLifetimeManager());
			#endregion

			#region Web
			container.RegisterType<IFactory<AppVersionHandler>, AppVersionHandlerFactory>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IEnvironmentProvider)));

			container.RegisterType<AppVersionHandler>(
				new HierarchicalLifetimeManager(),
				new InjectionFactory(c => c.Resolve<IFactory<AppVersionHandler>>().Create()));

			container.RegisterType<IUrlEncoder, PercentUrlEncoder>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(
					new PercentUrlEncodingPipeline()));

			container.RegisterType<IQueryBuilder, EncodedQueryBuilder>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IUrlEncoder)));

			container.RegisterType<IDocumentLoader, HttpDocumentLoader>(
				new HierarchicalLifetimeManager());
			#endregion

			#region Data
			container.RegisterType<IModelContainer, ModelContainer>(
				new HierarchicalLifetimeManager());
			#endregion

			#region Validation
			container.RegisterType<IValidatorFactory, AttributedValidatorFactory>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IModelValidatorFactory, FluentModelValidatorFactory>(
				new HierarchicalLifetimeManager());

			container.RegisterType<ModelValidatorProvider, FluentModelValidatorProvider>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IValidatorFactory), typeof(IModelValidatorFactory)));
			#endregion

			#region VersionController
			container.RegisterType<IFactory<ApiVersion>, ApiVersionFactory>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IAssemblyProvider)));

			container.RegisterType<ApiVersion>(
				new HierarchicalLifetimeManager(),
				new InjectionFactory(c => c.Resolve<IFactory<ApiVersion>>().Create()));
			#endregion

			#region FoodController
			container.RegisterType<IFactory<AwsOptions>, AwsOptionsFactory>(
				new HierarchicalLifetimeManager());

			container.RegisterType<AwsOptions>(
				new HierarchicalLifetimeManager(),
				new InjectionFactory(c => c.Resolve<IFactory<AwsOptions>>().Create()));

			container.RegisterType<IArgumentBuilder, AwsArgumentBuilder>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(container.Resolve<AwsOptions>().AccessKey, container.Resolve<AwsOptions>().AssociateTag, typeof(IDateTimeProvider)));

			container.RegisterType<IFactory<HashAlgorithm>, AwsAlgorithmFactory>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(container.Resolve<AwsOptions>().SecretKey));

			container.RegisterType<IQuerySigner, AwsQuerySigner>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(container.Resolve<IFactory<HashAlgorithm>>().Create()));

			container.RegisterType<IUrlBuilder, AwsUrlBuilder>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IArgumentBuilder), typeof(IQueryBuilder), typeof(IQuerySigner)));

			container.RegisterType<IValidator<XElement>, XmlItemValidator>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IItemSelector, XmlItemSelector>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IValidator<XElement>)));

			container.RegisterType<IAwsClient, AwsClient>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IUrlBuilder), typeof(IDocumentLoader), typeof(IItemSelector)));

			container.RegisterType<IFoodDescriptorFactory, XmlFoodDescriptorFactory>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IFoodProvider, AzureFoodProvider>(
				"Azure",
				new HierarchicalLifetimeManager());

			container.RegisterType<IFoodProvider, AwsFoodProvider>(
				"Aws",
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IAwsClient), typeof(IFoodDescriptorFactory)));

			container.RegisterType<IFoodRepository, CompositeFoodRepository>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(container.ResolveAll<IFoodProvider>()));
			#endregion

			#region UserController
			container.RegisterType<IUserUnit, EntityUserUnit>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IModelContainer)));

			container.RegisterType<IUserRepository, EntityUserRepository>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IUserUnit)));
			#endregion

			#region RatingController
			container.RegisterType<IRatingUnit, EntityRatingUnit>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IModelContainer)));

			container.RegisterType<IRatingRepository, EntityRatingRepository>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IRatingUnit), typeof(IUserUnit)));
			#endregion
		}
	}
}