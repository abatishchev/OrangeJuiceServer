using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Validation;
using System.Xml.Linq;

using Ab;
using Ab.Amazon;
using Ab.Amazon.Configuration;
using Ab.Amazon.Cryptography;
using Ab.Amazon.Data;
using AwsProduct = Ab.Amazon.Data.Product;
using ProductSearchCriteria = Ab.Amazon.ProductSearchCriteria;
using Ab.Amazon.Filtering;
using Ab.Amazon.Pipeline;
using Ab.Amazon.Validation;
using Ab.Amazon.Web;
using Ab.Azure;
using Ab.Azure.Configuration;
using Ab.Configuration;
using Ab.Factory;
using Ab.Filtering;
using Ab.Pipeline;
using Ab.Reflection;
using Ab.Security;
using Ab.SimpleInjector;
using Ab.Threading;
using Ab.Validation;
using Ab.Web;
using Ab.WebApi.SimpleInjector;

using Drum;
using Elmah;

using FluentValidation.Attributes;
using FluentValidation.WebApi;

using Microsoft.WindowsAzure.Storage.Table;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Api.Infrastucture;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Security;
using OrangeJuice.Server.Services;

using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace OrangeJuice.Server.Api
{
	internal static class ContainerConfig
	{
		public static Container CreateWebApiContainer()
		{
			Container container = new Container();
			container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

			RegisterTypes(container);

			return container;
		}

		public static Container CreateOwinContainer()
		{
			Container container = new Container();
			container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

			container.RegisterSingleton<IConfigurationProvider, WebConfigurationProvider>();
			container.RegisterFactory<AuthOptions, AuthOptionsFactory>(Lifestyle.Singleton);

			return container;
		}

		private static void RegisterTypes(Container container)
		{
			#region Providers
			container.RegisterSingleton<IConfigurationProvider, WebConfigurationProvider>();

			container.RegisterSingleton<IEnvironmentProvider, ConfigurationEnvironmentProvider>();

			container.RegisterSingleton<IConnectionStringProvider, ConfigurationConnectionStringProvider>();

			container.RegisterSingleton<IDateTimeProvider, UtcDateTimeProvider>();

			container.RegisterSingleton<IAssemblyProvider, ReflectionAssemblyProvider>();
			#endregion

			#region Configuration
			container.RegisterFactory<AuthOptions, AuthOptionsFactory>(Lifestyle.Singleton);

			container.RegisterFactory<AzureOptions, AzureOptionsFactory>(Lifestyle.Singleton);

			container.Register<IConverter<DynamicTableEntity, AwsOptions>, DynamicAwsOptionsConverter>();
			container.Register<IOptionsProvider<AwsOptions>, AzureAwsOptionsProvider>();
			container.RegisterDecorator<IOptionsProvider<AwsOptions>, OptionsProviderAdapter<AwsOptions>>(Lifestyle.Singleton);
			container.RegisterFactory<AwsOptions, RoundrobinAwsOptionsFactory>(Lifestyle.Singleton);

			container.RegisterFactory<GoogleAuthOptions, GoogleAuthOptionsFactory>(Lifestyle.Singleton);
			#endregion

			#region Security
			container.RegisterFactory<X509Certificate2, X509Certificate2Factory>();
			container.RegisterFactory<Jwt, JwtFactory>();
			container.RegisterFactory<Task<AuthToken>, string, GoogleAuthTokenFactory>();
			container.RegisterFactory<Task<AuthToken>, AuthToken, AuthTokenFactory>();
			#endregion

			#region Web API
			// Filters
			container.RegisterCollection<IFilter>(new[] { typeof(WebApiContrib.Filters.ValidationAttribute) });

			// Handlers
			container.RegisterFactory<IValidator<HttpRequestMessage>, AcceptHeaderValidatorFactory>(Lifestyle.Singleton);
			container.RegisterCollection<DelegatingHandler>(new[] { typeof(DelegatingHandlerProxy<AppVersionHandler>) });

			//container.Register<ITraceRequestRepository, EntityTraceRequestRepository>();

			// Services
            container.Register<IAssembliesResolver>(() =>
				new Ab.WebApi.AssembliesResolver(
					//typeof(FSharp.Controllers.VersionController).Assembly,
					typeof(Controllers.VersionController).Assembly),
				Lifestyle.Singleton);
			container.Register<IHttpControllerTypeResolver>(() => new DefaultHttpControllerTypeResolver(), Lifestyle.Singleton);
			container.Register<IHttpControllerSelector>(() => new Ab.WebApi.HttpControllerSelector(GlobalConfiguration.Configuration), Lifestyle.Singleton);

			container.Register<IExceptionLogger, ElmahAggregateExceptionLogger>();

			ServiceCenter.Current = c => container;
			container.Register<ErrorLog>(() => new SqlErrorLog(container.GetInstance<IConnectionStringProvider>().GetDefaultConnectionString()));

			// Controllers
			container.RegisterWebApiControllers(GlobalConfiguration.Configuration, Assembly.GetExecutingAssembly());

			container.EnableHttpRequestMessageTracking(GlobalConfiguration.Configuration);
			container.Register<IFactory<HttpRequestMessage>>(() => new DelegateFactory<HttpRequestMessage>(container.GetCurrentHttpRequestMessage), Lifestyle.Singleton);
			container.RegisterSingleton<IUrlProvider, DrumUrlProvider>();
			#endregion

			#region Data
			container.RegisterWebApiRequest<IModelContext, ModelContext>();
			#endregion

			#region Fluent Validation
			container.Register<FluentValidation.IValidatorFactory, AttributedValidatorFactory>();
			container.Register<ModelValidatorProvider, FluentValidationModelValidatorProvider>();
			#endregion

			#region Azure
			container.Register<IBlobClient, AzureBlobClient>();

			container.Register<ITableClient, AzureTableClient>();

			container.Register<IAzureContainerClient, AzureContainerClient>();

			container.Register<IAzureClient, AzureClient>();

			container.Register<IConverter<string, AwsProduct>, JsonProductConverter>();

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

			container.RegisterSingleton<IScheduler>(Scheduler.Default);

			container.Register<IRequestScheduler, IntervalRequestScheduler>();

			container.Register<HttpClient>(() => HttpClientFactory.Create());
			container.Register<IHttpClient, HttpClientAdapter>();
			container.RegisterDecorator<IHttpClient, ThrottlingHttpClient>();

			container.Register<IValidator<XElement>, XmlRequestValidator>();

			container.Register<IItemSelector, XmlItemSelector>();

			container.Register<IPipeline<AwsProduct, XElement, ProductSearchCriteria>, ResponseGroupProductPipeline>();

			container.RegisterFactory<AwsProduct, XElement, ProductSearchCriteria, XmlProductFactory>();

			container.Register<IFilter<XElement>, PrimaryVariantlItemFilter>();

			container.Register<IAwsClient, XmlAwsClient>();

			container.Register<IAwsProductProvider, AwsProductProvider>();
			#endregion

			#region VersionController
			container.RegisterFactory<ApiVersion, ApiVersionFactory>();
			#endregion

			#region ProductController
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
			container.RegisterSingleton(uriMakerContext);
		}
	}
}