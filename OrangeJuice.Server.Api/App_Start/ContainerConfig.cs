using System;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using System.Xml.Linq;

using Drum;
using Elmah;
using Factory;

using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.WebApi;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Api.Handlers.Validation;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Security;
using OrangeJuice.Server.Services;
using OrangeJuice.Server.Threading;
using OrangeJuice.Server.Web;

using DefaultLifetimeManager = Microsoft.Practices.Unity.HierarchicalLifetimeManager;

using AuthOptionsFactory = OrangeJuice.Server.FSharp.Configuration.AuthOptionsFactory;
using AwsOptionsFactory = OrangeJuice.Server.FSharp.Configuration.AwsOptionsFactory;
using AzureOptionsFactory = OrangeJuice.Server.FSharp.Configuration.AzureOptionsFactory;

using ApiVersionFactory = OrangeJuice.Server.FSharp.Data.ApiVersionFactory;
using JsonProductDescriptorConverter = OrangeJuice.Server.FSharp.Data.JsonProductDescriptorConverter;
using XmlProductDescriptorFactory = OrangeJuice.Server.FSharp.Data.XmlProductDescriptorFactory;

using AwsAlgorithmFactory = OrangeJuice.Server.FSharp.Services.AwsAlgorithmFactory;
using AzureProductProvider = OrangeJuice.Server.FSharp.Services.AzureProductProvider;
using CachingCloudProductService = OrangeJuice.Server.FSharp.Services.CachingCloudProductService;
using JsonBlobNameResolver = OrangeJuice.Server.FSharp.Services.JsonBlobNameResolver;
using XmlAwsClient = OrangeJuice.Server.FSharp.Services.XmlAwsClient;

using XmlRequestValidator = OrangeJuice.Server.FSharp.Validation.XmlRequestValidator;

using EncodedQueryBuilder = OrangeJuice.Server.FSharp.Web.EncodedQueryBuilder;
using HttpClientAdapter = OrangeJuice.Server.FSharp.Web.HttpClientAdapter;
using ThrottlingHttpClient = OrangeJuice.Server.FSharp.Web.ThrottlingHttpClient;

namespace OrangeJuice.Server.Api
{
	internal static class ContainerConfig
	{
		public static IUnityContainer CreateWebApiContainer()
		{
			IUnityContainer container = new UnityContainer();

			RegisterTypes(container);

			return container;
		}

		public static IUnityContainer CreateOwinContainer()
		{
			IUnityContainer container = new UnityContainer();

			container.RegisterType<IConfigurationProvider, WebConfigurationProvider>(
				new DefaultLifetimeManager());

			container.RegisterFactory<AuthOptions, AuthOptionsFactory>(
				new DefaultLifetimeManager());

			return container;
		}

		/// <remarks>
		/// TransientLifetimeManager = new instance every time
		/// ContainerControlledLifetimeManager = singleton
		/// HierarchicalLifetimeManager = per request
		/// </remarks>
		private static void RegisterTypes(IUnityContainer container)
		{
			#region Providers
			container.RegisterType<IConfigurationProvider, WebConfigurationProvider>(
				new DefaultLifetimeManager());

			container.RegisterType<IEnvironmentProvider, ConfigurationEnvironmentProvider>(
				new DefaultLifetimeManager());

			container.RegisterType<IConnectionStringProvider, ConfigurationConnectionStringProvider>(
				new DefaultLifetimeManager());

			container.RegisterType<IDateTimeProvider, UtcDateTimeProvider>(
				new DefaultLifetimeManager());

			container.RegisterType<IAssemblyProvider, ReflectionAssemblyProvider>(
				new DefaultLifetimeManager());
			#endregion

			#region Configuration
			container.RegisterFactory<AuthOptions, AuthOptionsFactory>(
				new ContainerControlledLifetimeManager());

			container.RegisterFactory<AzureOptions, AzureOptionsFactory>(
				new ContainerControlledLifetimeManager());

			container.RegisterFactory<AwsOptions, AwsOptionsFactory>(
				new ContainerControlledLifetimeManager());

			container.RegisterFactory<GoogleAuthOptions, GoogleAuthOptionsFactory>(
				new ContainerControlledLifetimeManager());
			#endregion

			#region Security
			container.RegisterType<IFactory<string>, JwtFactory>(
				typeof(JwtFactory).Name, // named registration
				new DefaultLifetimeManager());
			container.RegisterFactory<Task<AuthToken>, string, GoogleAuthTokenFactory>(
				typeof(GoogleAuthTokenFactory).Name, // named registration
				new DefaultLifetimeManager(),
				new InjectionConstructor(
					new ResolvedParameter<IFactory<string>>(typeof(JwtFactory).Name))); // param1: named registration
			container.RegisterFactory<Task<AuthToken>, AuthToken, AuthTokenFactory>(
				typeof(AuthTokenFactory).Name, // named registration
				new DefaultLifetimeManager());
			#endregion

			#region Web API
			// Filters
			container.RegisterType<IFilter, WebApiContrib.Filters.ValidationAttribute>(
				typeof(WebApiContrib.Filters.ValidationAttribute).Name, // named registration
				new DefaultLifetimeManager());

			// Handlers
			//container.RegisterFactory<IValidator<HttpRequestMessage, string>, AccessTokenValidatorFactory>(
			//	typeof(AccessTokenAuthorizationHandler).Name, // named registration
			//	new DefaultLifetimeManager());
			//container.RegisterType<ISecurityTokenValidator, JwtSecurityTokenHandler>(
			//	new DefaultLifetimeManager());
			//container.RegisterFactory<TokenValidationParameters, TokenValidationParametersFactory>(
			//	new ContainerControlledLifetimeManager()); container.RegisterFactory<IFactory<IPrincipal, string>, PrincipalFactoryFactory>(
			//	new ContainerControlledLifetimeManager());
			//container.RegisterType<DelegatingHandler, AccessTokenAuthorizationHandler>(
			//	typeof(AccessTokenAuthorizationHandler).Name, // named registration
			//	new DefaultLifetimeManager(),
			//	new InjectionConstructor(
			//		new ResolvedParameter<IValidator<HttpRequestMessage, string>>(typeof(AccessTokenAuthorizationHandler).Name), // param1: named registration
			//		new ResolvedParameter<IFactory<IPrincipal, string>>())); // param2

			container.RegisterType<CurrentRequest>(
				new DefaultLifetimeManager());
			container.RegisterType<DelegatingHandler, CurrentRequestHandler>(
				typeof(CurrentRequestHandler).Name, // named registration
				new DefaultLifetimeManager());

			container.RegisterFactory<IValidator<HttpRequestMessage>, AppVersionValidatorFactory>(
				typeof(AppVersionHandler).Name, // named registration
				new DefaultLifetimeManager());
			container.RegisterType<DelegatingHandler, AppVersionHandler>(
				typeof(AppVersionHandler).Name, // named registration
				new DefaultLifetimeManager(),
				new InjectionConstructor(
					new ResolvedParameter<IValidator<HttpRequestMessage>>(typeof(AppVersionHandler).Name))); // param1: named registration

			container.RegisterType<ITraceRequestRepository, EntityTraceRequestRepository>(
				new DefaultLifetimeManager());
			container.RegisterType<DelegatingHandler, TraceRequestHandler>(
				typeof(TraceRequestHandler).Name, // named registration
				new DefaultLifetimeManager());

			// Services
			container.RegisterType<IExceptionLogger, Elmah.Contrib.WebApi.ElmahExceptionLogger>(
				new DefaultLifetimeManager());

			container.RegisterType<IServiceProvider, ContainerServiceProvider>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container));
			ServiceCenter.Current = c => container.Resolve<IServiceProvider>();
			container.RegisterType<ErrorLog, SqlErrorLog>(
				new DefaultLifetimeManager(),
				new InjectionFactory(c => new SqlErrorLog(c.Resolve<IConnectionStringProvider>().GetDefaultConnectionString())));

			container.RegisterType<HttpRequestMessage>(
				new DefaultLifetimeManager(),
				new InjectionFactory(c => c.Resolve<CurrentRequest>().Value));
			container.RegisterType(typeof(UriMaker<>),
				new DefaultLifetimeManager(),
				new InjectionConstructor(typeof(UriMakerContext), typeof(HttpRequestMessage)));
			#endregion

			#region Data
			container.RegisterType<IModelContext, ModelContext>(
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
			container.RegisterType<IBlobNameResolver, JsonBlobNameResolver>(
				new DefaultLifetimeManager());

			container.RegisterType<IBlobClient, AzureBlobClient>(
				new DefaultLifetimeManager());

			container.RegisterType<IAzureClient, AzureClient>(
				new DefaultLifetimeManager());

			container.RegisterType<IConverter<string, ProductDescriptor>, JsonProductDescriptorConverter>(
				new DefaultLifetimeManager());

			container.RegisterType<IAzureProductProvider, AzureProductProvider>(
				new DefaultLifetimeManager());
			#endregion

			#region Aws
			container.RegisterType<IArgumentBuilder, AwsArgumentBuilder>(
				new DefaultLifetimeManager());

			container.RegisterType<IPipeline<string>, PercentUrlEncodingPipeline>(
				typeof(PercentUrlEncodingPipeline).Name, // named registration
				new DefaultLifetimeManager());

			container.RegisterType<IUrlEncoder, PercentUrlEncoder>(
				new DefaultLifetimeManager(),
				new InjectionConstructor(
					container.Resolve(typeof(IPipeline<string>),
					typeof(PercentUrlEncodingPipeline).Name)));  // named registration

			container.RegisterType<IQueryBuilder, EncodedQueryBuilder>(
				new DefaultLifetimeManager());

			container.RegisterFactory<System.Security.Cryptography.HashAlgorithm, AwsAlgorithmFactory>(
				new DefaultLifetimeManager());

			container.RegisterType<IQuerySigner, AwsQuerySigner>(
				new DefaultLifetimeManager());

			container.RegisterType<IUrlBuilder, AwsUrlBuilder>(
				new DefaultLifetimeManager());

			container.RegisterType<IScheduler>(
				new ContainerControlledLifetimeManager(),
				new InjectionFactory(c => Scheduler.Default));

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

			container.RegisterFactory<ProductDescriptor, XElement, XmlProductDescriptorFactory>(
				new DefaultLifetimeManager());

			container.RegisterType<IAwsClient, XmlAwsClient>(
				new DefaultLifetimeManager());

			container.RegisterType<IAwsProductProvider, AwsProductProvider>(
				new DefaultLifetimeManager());
			#endregion

			container.RegisterType<IProductRepository, EntityProductRepository>(
				new HierarchicalLifetimeManager());

			container.RegisterType<IProductService, CachingCloudProductService>(
				new HierarchicalLifetimeManager());
			#endregion

			#region UserController
			container.RegisterType<IUserRepository, EntityUserRepository>(
				new HierarchicalLifetimeManager());
			#endregion

			#region RatingController
			container.RegisterType<IRatingRepository, EntityRatingRepository>(
				new HierarchicalLifetimeManager());
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
		public static IUnityContainer RegisterFactory<T, TFactory>(this IUnityContainer container, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
			where TFactory : IFactory<T>
		{
			return container.RegisterType<IFactory<T>, TFactory>(
								new ContainerControlledLifetimeManager(), // singleton
								injectionMembers)
							.RegisterType<T>(
								name,
								lifetimeManager,
								new InjectionFactory(c => c.Resolve<IFactory<T>>().Create()));
		}

		public static IUnityContainer RegisterFactory<T, TFactory>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
			where TFactory : IFactory<T>
		{
			return container.RegisterFactory<T, TFactory>(null, lifetimeManager, injectionMembers);
		}

		public static IUnityContainer RegisterFactory<T, TArg, TFactory>(this IUnityContainer container, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
			where TFactory : IFactory<T, TArg>
		{
			return container.RegisterType<IFactory<T, TArg>, TFactory>(
								name,
								new ContainerControlledLifetimeManager(), // singleton
								injectionMembers);
		}

		public static IUnityContainer RegisterFactory<T, TArg, TFactory>(this IUnityContainer container, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
			where TFactory : IFactory<T, TArg>
		{
			return container.RegisterFactory<T, TArg, TFactory>(null, lifetimeManager, injectionMembers);
		}
	}

	// TODO: extract into nuget package
	internal class ContainerServiceProvider : IServiceProvider
	{
		private readonly IUnityContainer _container;

		public ContainerServiceProvider(IUnityContainer container)
		{
			_container = container;
		}

		public object GetService(Type serviceType)
		{
			return _container.Resolve(serviceType);
		}
	}
}