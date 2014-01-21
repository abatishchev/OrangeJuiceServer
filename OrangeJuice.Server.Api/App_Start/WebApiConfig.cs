using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Validation;

using Microsoft.Practices.Unity;

using Newtonsoft.Json;

using OrangeJuice.Server.Api.Handlers;
using OrangeJuice.Server.Configuration;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	internal static class WebApiConfig
	{
		public static void Configure(HttpConfiguration config, IUnityContainer container)
		{
			ConfigureServices(config.Services, container);
			ConfigureHandlers(config.MessageHandlers, container);

			ConfigureErrorDetailPolicy(config, container);
			ConfigureFormatters(config.Formatters);

			// Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
			// To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
			// For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
			//config.EnableQuerySupport();

			// To disable tracing in your application, please comment out or remove the following line of code
			//config.EnableSystemDiagnosticsTracing();
		}

		private static void ConfigureServices(ServicesContainer services, IUnityContainer container)
		{
			services.Replace(typeof(ModelValidatorProvider), container.Resolve<ModelValidatorProvider>());

			services.Add(typeof(IExceptionLogger), new Services.ElmahExceptionLogger());
		}

		private static void ConfigureHandlers(ICollection<DelegatingHandler> handlers, IUnityContainer container)
		{
			DelegatingHandler handler = container.Resolve<AppVersionHandler>();
			handlers.Add(handler);
		}

		private static void ConfigureErrorDetailPolicy(HttpConfiguration config, IUnityContainer container)
		{
			IEnvironmentProvider environmentProvider = container.Resolve<IEnvironmentProvider>();
			config.IncludeErrorDetailPolicy = new Policies.ErrorDetailPolicyResolver(environmentProvider).Resolve();
		}

		private static void ConfigureFormatters(MediaTypeFormatterCollection formatters)
		{
			formatters.Remove(formatters.XmlFormatter);
			formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;

			var settings = formatters.JsonFormatter.SerializerSettings;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
			settings.PreserveReferencesHandling = PreserveReferencesHandling.Arrays;
			settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		}
	}
}