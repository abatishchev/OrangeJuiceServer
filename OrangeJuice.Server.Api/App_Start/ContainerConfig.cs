using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using System.Xml.Linq;

using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.WebApi;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Api.Filters;
using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Api.Handlers.Validation;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Container;
using OrangeJuice.Server.Data.Repository;
using OrangeJuice.Server.Data.Unit;
using OrangeJuice.Server.Services;
using OrangeJuice.Server.Validation;
using OrangeJuice.Server.Web;

using DefaultLifetimeManager = Microsoft.Practices.Unity.TransientLifetimeManager;

namespace OrangeJuice.Server.Api
{
	internal static class ContainerConfig
	{
		public static IUnityContainer CreateContainer()
		{
			IUnityContainer container = new UnityContainer();

			RegisterTypes(container);

			// Web API
			GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

			return container;
		}

		/// <remarks>
		/// TransientLifetimeManager = new instance every time
		/// ContainerControlledLifetimeManager  = singleton
		/// HierarchicalLifetimeManager = child container
		/// </remarks>
		private static void RegisterTypes(IUnityContainer container)
		{
			#region Web API
			// Filters
			container.RegisterType<IFilter, ValidModelActionFilter>(
				typeof(ValidModelActionFilter).Name,
				new DefaultLifetimeManager());

			// Handlers
			container.RegisterType<IFactory<IValidator<HttpRequestMessage>>, AppVersionValidatorFactory>(
				new DefaultLifetimeManager(),
				new InjectionConstructor(typeof(IEnvironmentProvider)));

			container.RegisterType<DelegatingHandler, AppVersionHandler>(
				new DefaultLifetimeManager(),
				new InjectionConstructor(typeof(IValidator<HttpRequestMessage>)));

			// Services
			container.RegisterType<IExceptionLogger, Elmah.Contrib.WebApi.ElmahExceptionLogger>(
				new DefaultLifetimeManager());
			#endregion

			#region Providers
			container.RegisterType<IConfigurationProvider, CloudConfigurationProvider>(
				new DefaultLifetimeManager());

			container.RegisterType<IEnvironmentProvider, ConfigurationEnvironmentProvider>(
				new DefaultLifetimeManager(),
				new InjectionConstructor(typeof(IConfigurationProvider)));

			container.RegisterType<IDateTimeProvider, UtcDateTimeProvider>(
				new DefaultLifetimeManager());

			container.RegisterType<IAssemblyProvider, ReflectionAssemblyProvider>(
				new DefaultLifetimeManager());
			#endregion

			#region Data
			container.RegisterType<IModelContainer, ModelContainer>(
				new HierarchicalLifetimeManager());
			#endregion

			#region Validation
			container.RegisterType<IValidatorFactory, AttributedValidatorFactory>(
				new DefaultLifetimeManager());

		  		container.RegisterType<ModelValidatorProvider, FluentValidationModelValidatorProvider>(
				new DefaultLifetimeManager());
			#endregion

			#region VersionController
			container.RegisterFactory<ApiVersion, ApiVersionFactory>(
				new TransientLifetimeManager()); // create every time, don't need to keep it in memory
			#endregion

			#region ProductController
			#region Azure
			container.RegisterFactory<AzureOptions, AzureOptionsFactory>(
				new DefaultLifetimeManager());

			container.RegisterType<IBlobNameResolver, JsonBlobNameResolver>(
				new DefaultLifetimeManager());

			container.RegisterType<IBlobClient, AzureBlobClient>(
				new DefaultLifetimeManager());

			container.RegisterType<IAzureClient, AzureClient>(
				new DefaultLifetimeManager());

			container.RegisterType<IConverter<string, ProductDescriptor>, StringProductDescriptorConverter>(
				new DefaultLifetimeManager());

			container.RegisterType<IAzureProductProvider, AzureProductProvider>(
				new DefaultLifetimeManager());
			#endregion

			#region Aws
			container.RegisterFactory<AwsOptions, AwsOptionsFactory>(
				new DefaultLifetimeManager());

			container.RegisterType<IArgumentBuilder, AwsArgumentBuilder>(
				new DefaultLifetimeManager());

			container.RegisterType<IPipeline<string, string>, PercentUrlEncodingPipeline>(
				"percent",
				new DefaultLifetimeManager());

			container.RegisterType<IUrlEncoder, PercentUrlEncoder>(
				new DefaultLifetimeManager(),
				new InjectionConstructor(container.Resolve(typeof(IPipeline<string, string>), "percent")));

			container.RegisterType<IQueryBuilder, EncodedQueryBuilder>(
				new DefaultLifetimeManager());

			container.RegisterFactory<HashAlgorithm, AwsAlgorithmFactory>(
				new DefaultLifetimeManager());

			container.RegisterType<IQuerySigner, AwsQuerySigner>(
				new DefaultLifetimeManager());

			container.RegisterType<IUrlBuilder, AwsUrlBuilder>(
				new DefaultLifetimeManager());

			container.RegisterType<IHttpClient, Web.HttpClient>(
				new DefaultLifetimeManager());

			container.RegisterType<IValidator<XElement>, XmlRequestValidator>(
				new DefaultLifetimeManager());

			container.RegisterType<IItemSelector, XmlItemSelector>(
				new DefaultLifetimeManager());

			container.RegisterType<IAwsClient, XmlAwsClient>(
				new DefaultLifetimeManager());

			container.RegisterType<IFactory<XElement, ProductDescriptor>, XmlProductDescriptorFactory>(
				new DefaultLifetimeManager());

			container.RegisterType<IAwsProductProvider, AwsProductProvider>(
				new DefaultLifetimeManager());
			#endregion

			container.RegisterType<IProductUnit, EntityProductUnit>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IProductRepository, EntityProductRepository>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IProductService, CloudProductService>(
				new HierarchicalLifetimeManager());
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
				new InjectionConstructor(typeof(IRatingUnit)));
			#endregion
		}
	}

	internal static class UnityContainerExtensions
	{
		public static IUnityContainer RegisterFactory<T, TFactory>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
			where TFactory : IFactory<T>
		{
			return container.RegisterType<IFactory<T>, TFactory>(
				new ContainerControlledLifetimeManager(), // singleton
				injectionMembers)
							.RegisterType<T>(
								lifetimeManager,
								new InjectionFactory(c => c.Resolve<IFactory<T>>().Create()));
		}
	}
}