using System;
using System.Net.Http;
using System.Web.Http;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	internal static class ControllerFactory<T> where T : ApiController
	{
		public static T Create(params object[] args) 
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