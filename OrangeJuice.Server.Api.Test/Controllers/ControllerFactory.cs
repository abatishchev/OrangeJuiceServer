using System;
using System.Net.Http;
using System.Web.Http;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	internal static class ControllerFactory
	{
		public static T Create<T>(params object[] args) where T : ApiController
		{
			var config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			config.EnsureInitialized();

			var request = new HttpRequestMessage();
			request.SetConfiguration(config);
			request.ShouldIncludeErrorDetail();

			T controller = (T)Activator.CreateInstance(typeof(T), args);
			controller.Request = request;
			controller.RequestContext.IncludeErrorDetail = true;
			return controller;
		}
	}
}