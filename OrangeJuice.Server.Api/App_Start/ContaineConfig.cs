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
using OrangeJuice.Server.Api.Policies;
using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Container;
using OrangeJuice.Server.Data.Repository;
using OrangeJuice.Server.Data.Unit;
using OrangeJuice.Server.Services;
using OrangeJuice.Server.Validation;
using OrangeJuice.Server.Web;

namespace OrangeJuice.Server.Api
{
	internal static class ContaineConfig
	{
		public static IUnityContainer CreateContainer()
		{
			IUnityContainer container = new UnityContainer();

			RegisterTypes(container);

			// Web API
			GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

			return container;
		}

		private static void RegisterTypes(IUnityContainer container)
		{
			#region Web API
			// Filters
			container.RegisterType<IFilter, ValidModelActionFilter>(
				typeof(ValidModelActionFilter).Name,
				new ContainerControlledLifetimeManager());

			// Handlers
			container.RegisterType<IFactory<IValidator<HttpRequestMessage>>, AppVersionValidatorFactory>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IEnvironmentProvider)));

			container.RegisterType<DelegatingHandler, AppVersionHandler>(
				new ContainerControlledLifetimeManager());

			// Services
			container.RegisterType<IExceptionLogger, Elmah.Contrib.WebApi.ElmahExceptionLogger>(
				new ContainerControlledLifetimeManager());
			#endregion

			#region Providers
			container.RegisterType<IConfigurationProvider, CloudConfigurationProvider>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IEnvironmentProvider, ConfigurationEnvironmentProvider>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IConfigurationProvider)));

			container.RegisterType<IDateTimeProvider, UtcDateTimeProvider>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IAssemblyProvider, ReflectionAssemblyProvider>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IErrorDetailPolicyProvider, EnvironmentErrorDetailPolicyProvider>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IFactory<IncludeErrorDetailPolicy>, ErrorDetailPolicyFactory>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IErrorDetailPolicyProvider), typeof(IEnvironmentProvider)));

			container.RegisterFactory<IncludeErrorDetailPolicy>(
				new ContainerControlledLifetimeManager());
			#endregion

			#region Data
			container.RegisterType<IModelContainer, ModelContainer>(
				new HierarchicalLifetimeManager());
			#endregion

			#region Validation
			container.RegisterType<IValidatorFactory, AttributedValidatorFactory>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<ModelValidatorProvider, FluentValidationModelValidatorProvider>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IValidatorFactory)));
			#endregion

			#region VersionController
			container.RegisterType<IFactory<ApiVersion>, ApiVersionFactory>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IAssemblyProvider), typeof(IEnvironmentProvider)));

			container.RegisterFactory<ApiVersion>(
				new HierarchicalLifetimeManager());
			#endregion

			#region ProductController
			#region Azure
			container.RegisterType<IFactory<AzureOptions>, AzureOptionsFactory>(
				new ContainerControlledLifetimeManager());

			container.RegisterFactory<AzureOptions>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IBlobNameResolver, JsonBlobNameResolver>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IBlobClient, AzureBlobClient>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IAzureClient, AzureClient>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(AzureOptions), typeof(IBlobNameResolver), typeof(IBlobClient)));

			container.RegisterType<IConverter<string, ProductDescriptor>, StringProductDescriptorConverter>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IAzureProductProvider, AzureProductProvider>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(AzureOptions), typeof(IAzureClient), typeof(IConverter<string, ProductDescriptor>)));
			#endregion

			#region Aws
			container.RegisterType<IFactory<AwsOptions>, AwsOptionsFactory>(
				new ContainerControlledLifetimeManager());

			container.RegisterFactory<AwsOptions>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IArgumentBuilder, AwsArgumentBuilder>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(AwsOptions), typeof(IDateTimeProvider)));

			container.RegisterType<IFactory<HashAlgorithm>, AwsAlgorithmFactory>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(AwsOptions)));

			container.RegisterType<IUrlEncoder, PercentUrlEncoder>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(
					new PercentUrlEncodingPipeline()));

			container.RegisterType<IQueryBuilder, EncodedQueryBuilder>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IUrlEncoder)));

			container.RegisterType<IQuerySigner, AwsQuerySigner>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(container.Resolve<IFactory<HashAlgorithm>>().Create()));

			container.RegisterType<IUrlBuilder, AwsUrlBuilder>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IArgumentBuilder), typeof(IQueryBuilder), typeof(IQuerySigner)));

			container.RegisterType<IValidator<XElement>, XmlRequestValidator>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IDocumentLoader, HttpDocumentLoader>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IItemSelector, XmlItemSelector>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IValidator<XElement>)));

			container.RegisterType<IAwsClient, AwsClient>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IUrlBuilder), typeof(IDocumentLoader), typeof(IItemSelector)));

			container.RegisterType<IFactory<XElement, ProductDescriptor>, XmlProductDescriptorFactory>(
				new ContainerControlledLifetimeManager());

			container.RegisterType<IAwsProductProvider, AwsProductProvider>(
				new ContainerControlledLifetimeManager(),
				new InjectionConstructor(typeof(IAwsClient), typeof(IFactory<XElement, ProductDescriptor>)));
			#endregion

			container.RegisterType<IProductUnit, EntityProductUnit>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IModelContainer)));

			container.RegisterType<IProductRepository, EntityProductRepository>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IProductUnit)));

			container.RegisterType<IProductService, CloudProductService>(
				new HierarchicalLifetimeManager(),
				new InjectionConstructor(typeof(IProductRepository), typeof(IAzureProductProvider), typeof(IAwsProductProvider)));
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
}