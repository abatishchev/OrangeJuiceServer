using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using System.Web.Http.Validation;

using Drum;

using Microsoft.Practices.Unity;

using Newtonsoft.Json;

using WebApiContrib.Configuration;

namespace OrangeJuice.Server.Api
{
	internal static class WebApiConfig
	{
		public static void Configure(HttpConfiguration config, IUnityContainer container)
		{
			config.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);

			ConfigureFilters(config.Filters, container);
			ConfigureHandlers(config.MessageHandlers, container);
			ConfigureServices(config.Services, container);
			ConfigureFormatters(config.Formatters);

			config.UseWebConfigCustomErrors();

			//config.MapHttpAttributeRoutes();
			var uriMaker = config.MapHttpAttributeRoutesAndUseUriMaker();
			ContainerConfig.RegisterUriMaker(container, uriMaker);

			// Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
			// To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
			// For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
			//config.EnableQuerySupport();

			// To disable tracing in your application, please comment out or remove the following line of code
			//config.EnableSystemDiagnosticsTracing();
		}

		public static void ConfigureFilters(HttpFilterCollection filters, IUnityContainer container)
		{
			filters.AddRange(container.ResolveAll<IFilter>());
		}

		private static void ConfigureHandlers(ICollection<DelegatingHandler> handlers, IUnityContainer container)
		{
			foreach (DelegatingHandler handler in container.ResolveAll<DelegatingHandler>())
			{
				handlers.Add(handler);
			}
		}

		private static void ConfigureServices(ServicesContainer services, IUnityContainer container)
		{
			services.Replace(typeof(ModelValidatorProvider), container.Resolve<ModelValidatorProvider>());

			services.Add(typeof(IExceptionLogger), container.Resolve<IExceptionLogger>());
		}

		private static void ConfigureFormatters(MediaTypeFormatterCollection formatters)
		{
			formatters.Remove(formatters.XmlFormatter);
			formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

			var settings = formatters.JsonFormatter.SerializerSettings;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
			settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		}
	}
}