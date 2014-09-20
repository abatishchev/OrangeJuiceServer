using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using System.Xml.Linq;

using Drum;

using Factory;

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
using OrangeJuice.Server.Threading;
using OrangeJuice.Server.Validation;
using OrangeJuice.Server.Web;

using DefaultLifetimeManager = Microsoft.Practices.Unity.HierarchicalLifetimeManager;

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
		/// ContainerControlledLifetimeManager = singleton
		/// HierarchicalLifetimeManager = per request
		/// </remarks>
		private static void RegisterTypes(IUnityContainer container)
		{
			#region Web API
			// Filters
			container.RegisterType<IFilter, ValidModelActionFilter>(
				typeof(ValidModelActionFilter).Name, // named registration
				new DefaultLifetimeManager());

			// Handlers
			container.RegisterType<CurrentRequest>(
				new DefaultLifetimeManager());
			container.RegisterType<DelegatingHandler, CurrentRequestHandler>(
				typeof(CurrentRequestHandler).Name, // named registration
				new DefaultLifetimeManager());

			container.RegisterFactory<IValidator<HttpRequestMessage>, AppVersionValidatorFactory>(
				new DefaultLifetimeManager());
			container.RegisterType<DelegatingHandler, AppVersionHandler>(
				typeof(AppVersionHandler).Name, // named registration
				new DefaultLifetimeManager());

			// Services
			container.RegisterType<IExceptionLogger, Elmah.Contrib.WebApi.ElmahExceptionLogger>(
				new DefaultLifetimeManager());

			container.RegisterType<HttpRequestMessage>(
				new DefaultLifetimeManager(),
				new InjectionFactory(c => c.Resolve<CurrentRequest>().Value));
			container.RegisterType(typeof(UriMaker<>),
				new DefaultLifetimeManager(),
				new InjectionConstructor(typeof(UriMakerContext), typeof(HttpRequestMessage)));
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
				new TransientLifetimeManager()); // new instance every time
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
				typeof(PercentUrlEncodingPipeline).Name, // named registration
				new DefaultLifetimeManager());

			container.RegisterType<IUrlEncoder, PercentUrlEncoder>(
				new DefaultLifetimeManager(),
				new InjectionConstructor(container.Resolve(typeof(IPipeline<string, string>), typeof(PercentUrlEncodingPipeline).Name)));  // named registration

			container.RegisterType<IQueryBuilder, EncodedQueryBuilder>(
				new DefaultLifetimeManager());

			container.RegisterFactory<System.Security.Cryptography.HashAlgorithm, AwsAlgorithmFactory>(
				new DefaultLifetimeManager());

			container.RegisterType<IQuerySigner, AwsQuerySigner>(
				new DefaultLifetimeManager());

			container.RegisterType<IUrlBuilder, AwsUrlBuilder>(
				new DefaultLifetimeManager());

			container.RegisterType<IRequestScheduler, IntervalRequestScheduler>(
				new ContainerControlledLifetimeManager()); // singleton

			container.RegisterType<HttpClient>(
				new TransientLifetimeManager(), // new instance
				new InjectionFactory(c => HttpClientFactory.Create()));

			container.RegisterType<IHttpClient, HttpClientAdapter>(
				typeof(HttpClientAdapter).Name, // named registration
				new DefaultLifetimeManager());

			container.RegisterType<IHttpClient, ThrottlingHttpClient>(
				new DefaultLifetimeManager(),
				new InjectionConstructor(
					new ResolvedParameter(typeof(IHttpClient), (typeof(HttpClientAdapter).Name)),
					typeof(IRequestScheduler))); // named resolving

			container.RegisterType<IValidator<XElement>, XmlRequestValidator>(
				new DefaultLifetimeManager());

			container.RegisterType<IItemSelector, XmlItemSelector>(
				new DefaultLifetimeManager());

			container.RegisterType<IAwsClient, XmlAwsClient>(
				new DefaultLifetimeManager());

			container.RegisterFactory<ProductDescriptor, XElement, XmlProductDescriptorFactory>(
				new DefaultLifetimeManager());

			container.RegisterType<IAwsProductProvider, AwsProductProvider>(
				new DefaultLifetimeManager());
			#endregion

			container.RegisterType<IProductUnit, EntityProductUnit>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IProductRepository, EntityProductRepository>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IProductService, AwsProductService>(
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

		internal static void RegisterUriMaker(IUnityContainer container, UriMakerContext uriMakerContext)
		{
			container.RegisterInstance(uriMakerContext,
				new ExternallyControlledLifetimeManager());
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

		public static IUnityContainer RegisterFactory<T, TArg, TFactory>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
			where TFactory : IFactory<T, TArg>
		{
			return container.RegisterType<IFactory<T, TArg>, TFactory>(
								new ContainerControlledLifetimeManager(), // singleton
								injectionMembers)
							.RegisterType<T>(
								lifetimeManager,
								new InjectionFactory(c => c.Resolve<IFactory<T>>().Create()));
		}
	}
}