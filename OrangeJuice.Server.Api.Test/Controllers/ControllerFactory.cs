using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	internal static class ControllerFactory
	{
		public static T Create<T>(params object[] args) where T : ApiController
		{
			var config = new HttpConfiguration
			{
				IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always
			};
			var request = new HttpRequestMessage();
			var routeData = new HttpRouteData(new HttpRoute());

			T controller = (T)Activator.CreateInstance(typeof(T), args);
			controller.ControllerContext = new HttpControllerContext(config, routeData, request);
			controller.Request = request;
			controller.Request.Properties.Add(System.Web.Http.Hosting.HttpPropertyKeys.HttpConfigurationKey, config);
			return controller;
		}
	}
}