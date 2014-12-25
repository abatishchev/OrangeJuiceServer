using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Validation;

using Drum;

using Newtonsoft.Json;

using SimpleInjector;

using WebApiContrib.Configuration;

namespace OrangeJuice.Server.Api
{
	internal static class WebApiConfig
	{
		public static void Configure(HttpConfiguration config, Container container)
		{
			config.DependencyResolver = new SimpleInjector.Integration.WebApi.SimpleInjectorWebApiDependencyResolver(container);

			//config.MapHttpAttributeRoutes();
			var uriMaker = config.MapHttpAttributeRoutesAndUseUriMaker();
			ContainerConfig.RegisterUriMaker(container, uriMaker);
			container.Verify();

			config.UseWebConfigCustomErrors();

			ConfigureFilters(config.Filters, container);
			ConfigureHandlers(config.MessageHandlers, container);
			ConfigureServices(config.Services, container);
			ConfigureFormatters(config.Formatters);

			// Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
			// To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
			// For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
			//config.EnableQuerySupport();

			// To disable tracing in your application, please comment out or remove the following line of code
			//config.EnableSystemDiagnosticsTracing();
		}

		public static void ConfigureFilters(HttpFilterCollection filters, Container container)
		{
			filters.AddRange(container.GetAllInstances<IFilter>());
		}

		private static void ConfigureHandlers(ICollection<DelegatingHandler> handlers, Container container)
		{
			foreach (DelegatingHandler handler in container.GetAllInstances<DelegatingHandler>())
			{
				handlers.Add(handler);
			}
		}

		private static void ConfigureServices(ServicesContainer services, Container container)
		{
			container.ReplaceService<IAssembliesResolver>(services);
			container.ReplaceService<IHttpControllerTypeResolver>(services);

			container.ReplaceService<ModelValidatorProvider>(services);

			container.AddService<IExceptionLogger>(services);
		}

		private static void ConfigureFormatters(MediaTypeFormatterCollection formatters)
		{
			formatters.Remove(formatters.XmlFormatter);

			var settings = formatters.JsonFormatter.SerializerSettings;
			settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
			settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
			settings.Formatting = Formatting.Indented;
			settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		}
	}
}