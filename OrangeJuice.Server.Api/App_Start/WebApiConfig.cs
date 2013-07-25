﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

using Newtonsoft.Json;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			ConfigureHandlers(config.MessageHandlers);

			ConfigureFormatters(config.Formatters);

			// Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
			// To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
			// For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
			//config.EnableQuerySupport();

			// To disable tracing in your application, please comment out or remove the following line of code
			// For more information, refer to: http://www.asp.net/web-api
			//config.EnableSystemDiagnosticsTracing();
		}

		private static void ConfigureHandlers(ICollection<DelegatingHandler> messageHandlers)
		{
		}

		private static void ConfigureFormatters(MediaTypeFormatterCollection formatters)
		{
			formatters.Remove(formatters.XmlFormatter);

			var jsonSerializerSettings = formatters.JsonFormatter.SerializerSettings;
			jsonSerializerSettings.Formatting = Formatting.Indented;
			jsonSerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

			formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
		}
	}
}