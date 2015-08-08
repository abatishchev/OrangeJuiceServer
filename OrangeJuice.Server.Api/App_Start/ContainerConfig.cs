using System.Net.Http;
using System.Reactive.Concurrency;
using System.Runtime.Caching;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
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

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Api.Handlers.Validation;
using OrangeJuice.Server.Api.Infrastucture;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Services;
using OrangeJuice.Server.Threading;
using OrangeJuice.Server.Web;

using SimpleInjector;
using SimpleInjector.Extensions;

using AuthOptionsFactory = OrangeJuice.Server.FSharp.Configuration.AuthOptionsFactory;
using AwsOptionsFactory = OrangeJuice.Server.FSharp.Configuration.AwsOptionsFactory;
using AzureOptionsFactory = OrangeJuice.Server.FSharp.Configuration.AzureOptionsFactory;
using CachingConfigurationProvider = OrangeJuice.Server.FSharp.Configuration.CachingConfigurationProvider;
using ConfigurationConnectionStringProvider = OrangeJuice.Server.FSharp.Configuration.ConfigurationConnectionStringProvider;
using ConfigurationEnvironmentProvider = OrangeJuice.Server.FSharp.Configuration.ConfigurationEnvironmentProvider;
using WebConfigurationProvider = OrangeJuice.Server.FSharp.Configuration.WebConfigurationProvider;

using ApiVersionFactory = OrangeJuice.Server.FSharp.Data.ApiVersionFactory;
using JsonProductDescriptorConverter = OrangeJuice.Server.FSharp.Data.JsonProductDescriptorConverter;
using XmlProductDescriptorFactory = OrangeJuice.Server.Data.XmlProductDescriptorFactory;

using AuthTokenFactory = OrangeJuice.Server.FSharp.Security.AuthTokenFactory;
using JwtFactory = OrangeJuice.Server.Security.JwtFactory;
using GoogleAuthTokenFactory = OrangeJuice.Server.Security.GoogleAuthTokenFactory;

using AwsAlgorithmFactory = OrangeJuice.Server.FSharp.Services.AwsAlgorithmFactory;
using AwsArgumentBuilder = OrangeJuice.Server.FSharp.Services.AwsArgumentBuilder;
using AwsProductProvider = OrangeJuice.Server.FSharp.Services.AwsProductProvider;
using AzureClient = OrangeJuice.Server.FSharp.Services.AzureClient;
using AzureProductProvider = OrangeJuice.Server.FSharp.Services.AzureProductProvider;
using CachingCloudProductService = OrangeJuice.Server.FSharp.Services.CachingCloudProductService;
using XmlAwsClient = OrangeJuice.Server.FSharp.Services.XmlAwsClient;
using XmlItemSelector = OrangeJuice.Server.FSharp.Services.XmlItemSelector;

using XmlRequestValidator = OrangeJuice.Server.FSharp.Validation.XmlRequestValidator;

using EncodedQueryBuilder = OrangeJuice.Server.FSharp.Web.EncodedQueryBuilder;
using HttpClientAdapter = OrangeJuice.Server.FSharp.Web.HttpClientAdapter;
using ThrottlingHttpClient = OrangeJuice.Server.FSharp.Web.ThrottlingHttpClient;

namespace OrangeJuice.Server.Api
{
	internal static class ContainerConfig
	{
		public static Container CreateWebApiContainer(bool registerControllers = false)
		{
			Container container = new Container();

			RegisterTypes(container, registerControllers);

			return container;
		}

		public static Container CreateOwinContainer()
		{
			Container container = new Container();

			container.Register<IConfigurationProvider, WebConfigurationProvider>();
			container.RegisterSingle(MemoryCache.Default);
			container.RegisterSingleDecorator(typeof(IConfigurationProvider), typeof(CachingConfigurationProvider));
			container.RegisterFactory<AuthOptions, AuthOptionsFactory>();

			container.Verify();
			return container;
		}

		private static void RegisterTypes(Container container, bool registerControllers)
		{
			#region Providers
			container.Register<IConfigurationProvider, WebConfigurationProvider>();

			container.Register<IEnvironmentProvider, ConfigurationEnvironmentProvider>();

			container.Register<IConnectionStringProvider, ConfigurationConnectionStringProvider>();

			container.Register<IDateTimeProvider, UtcDateTimeProvider>();

			container.Register<IAssemblyProvider, ReflectionAssemblyProvider>();
			#endregion

			#region Configuration
			container.RegisterFactory<AuthOptions, AuthOptionsFactory>();

			container.RegisterFactory<AzureOptions, AzureOptionsFactory>();

			container.RegisterFactory<AwsOptions, AwsOptionsFactory>();

			container.RegisterFactory<GoogleAuthOptions, GoogleAuthOptionsFactory>();
			#endregion

			#region Security
			container.RegisterFactory<X509Certificate2, Security.X509Certificate2Factory>();
			container.Register<IFactory<string>, JwtFactory>();
			container.RegisterFactory<Task<AuthToken>, string, GoogleAuthTokenFactory>();
			container.RegisterFactory<Task<AuthToken>, AuthToken, AuthTokenFactory>();
			#endregion

			#region Web API
			// Filters
			container.RegisterAll<IFilter>(typeof(WebApiContrib.Filters.ValidationAttribute));

			// Handlers
			container.RegisterFactory<IValidator<HttpRequestMessage>, AppVersionValidatorFactory>();

			container.Register<ITraceRequestRepository, EntityTraceRequestRepository>();

			container.RegisterAll<DelegatingHandler>(typeof(DelegatingHandlerProxy<AppVersionHandler>));

			// Services
			container.Register<IAssembliesResolver>(() =>
				new AssembliesResolver(
					typeof(FSharp.Controllers.VersionController).Assembly,
					typeof(Controllers.VersionController).Assembly),
				Lifestyle.Singleton);
			container.Register<IHttpControllerTypeResolver>(() => new DefaultHttpControllerTypeResolver(), Lifestyle.Singleton);
			container.Register<IHttpControllerSelector>(() => new HttpControllerSelector(GlobalConfiguration.Configuration), Lifestyle.Singleton);

			container.Register<IExceptionLogger, Elmah.Contrib.WebApi.ElmahExceptionLogger>();

			ServiceCenter.Current = c => container;
			container.Register<ErrorLog>(() => new SqlErrorLog(container.GetInstance<IConnectionStringProvider>().GetDefaultConnectionString()));

			// Controllers
			if (registerControllers)
			{
				container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
			}

			container.EnableHttpRequestMessageTracking(GlobalConfiguration.Configuration);
			container.Register<IFactory<HttpRequestMessage>>(() => new DelegateFactory<HttpRequestMessage>(container.GetCurrentHttpRequestMessage));
			container.RegisterSingle<IUrlProvider, DrumUrlProvider>();
			#endregion

			#region Data
			container.RegisterWebApiRequest<IModelContext, ModelContext>();
			#endregion

			#region Validation
			container.Register<IValidatorFactory, AttributedValidatorFactory>();

			container.Register<ModelValidatorProvider, FluentValidationModelValidatorProvider>();
			#endregion

			#region VersionController
			container.RegisterFactory<ApiVersion, ApiVersionFactory>();
			#endregion

			#region ProductController
			#region Azure
			container.Register<IBlobClient, AzureBlobClient>();

			container.Register<IAzureClient, AzureClient>();

			container.Register<IConverter<string, ProductDescriptor>, JsonProductDescriptorConverter>();

			container.Register<IAzureProductProvider, AzureProductProvider>();
			#endregion

			#region Aws
			container.Register<IArgumentBuilder, AwsArgumentBuilder>();

			container.Register<IPipeline<string>, PercentUrlEncodingPipeline>();

			container.Register<IUrlEncoder, PercentUrlEncoder>();

			container.Register<IQueryBuilder, EncodedQueryBuilder>();

			container.RegisterFactory<System.Security.Cryptography.HashAlgorithm, AwsAlgorithmFactory>();

			container.Register<IQuerySigner, AwsQuerySigner>();

			container.Register<IUrlBuilder, AwsUrlBuilder>();

			container.RegisterSingle<IScheduler>(Scheduler.Default);

			container.Register<IRequestScheduler, IntervalRequestScheduler>();

			container.Register<HttpClient>(() => HttpClientFactory.Create());

			container.Register(typeof(IHttpClient), typeof(HttpClientAdapter));

			container.RegisterDecorator(typeof(IHttpClient), typeof(ThrottlingHttpClient));

			container.Register<IValidator<XElement>, XmlRequestValidator>();

			container.Register<IItemSelector, XmlItemSelector>();

			container.Register<IPipeline<ProductDescriptor, XElement, AwsProductSearchCriteria>, ResponseGroupProductDescriptorPipeline>();

			container.RegisterFactory<ProductDescriptor, XElement, AwsProductSearchCriteria, XmlProductDescriptorFactory>();

			container.Register<IAwsClient, XmlAwsClient>();

			container.Register<IAwsProductProvider, AwsProductProvider>();
			#endregion

			container.Register<IProductRepository, EntityProductRepository>();

			container.Register<IProductService, CachingCloudProductService>();
			#endregion

			#region UserController
			container.Register<IUserRepository, EntityUserRepository>();
			#endregion

			#region RatingController

			container.Register<IRatingRepository, EntityRatingRepository>();

			#endregion
		}

		public static void RegisterUriMaker(Container container, UriMakerContext uriMakerContext)
		{
			container.RegisterSingle(uriMakerContext);
		}
	}

	internal static class ContainerExtensions
	{
		public static void RegisterFactory<T, TFactory>(this Container container)
			where T : class
			where TFactory : class, IFactory<T>
		{
			container.Register<IFactory<T>, TFactory>(Lifestyle.Singleton);
			container.Register(() => container.GetInstance<TFactory>().Create());
		}

		public static void RegisterFactory<T, U, TFactory>(this Container container)
		   where TFactory : class, IFactory<T, U>
		{
			container.Register<IFactory<T, U>, TFactory>(Lifestyle.Singleton);
		}

		public static void RegisterFactory<T, U1, U2, TFactory>(this Container container)
		   where TFactory : class, IFactory<T, U1, U2>
		{
			container.Register<IFactory<T, U1, U2>, TFactory>(Lifestyle.Singleton);
		}

		public static void AddService<T>(this Container container, ServicesContainer services)
			where T : class
		{
			services.Add(typeof(T), container.GetInstance<T>());
		}

		public static void ReplaceService<T>(this Container container, ServicesContainer services)
			where T : class
		{
			services.Replace(typeof(T), container.GetInstance<T>());
		}
	}
}