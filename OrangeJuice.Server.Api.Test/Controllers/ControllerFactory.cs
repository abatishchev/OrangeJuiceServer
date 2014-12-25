using System;
using System.Net.Http;
using System.Web.Http;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	internal static class ControllerFactory
	{
		public static T Create<T>(params object[] args) where T : ApiController
		{
			return (T)Create(typeof(T), args);
		}

		public static ApiController Create(Type type, params object[] args)
		{
			var config = new HttpConfiguration();
			config.MapHttpAttributeRoutes();
			config.EnsureInitialized();

			var request = new HttpRequestMessage();
			request.SetConfiguration(config);
			request.ShouldIncludeErrorDetail();

			ApiController controller = (ApiController)Activator.CreateInstance(type, args);
			controller.Request = request;
			controller.RequestContext.IncludeErrorDetail = true;
			return controller;
		}
	}
}