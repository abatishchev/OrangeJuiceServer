using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	internal static class ControllerFactory
	{
		public static T Create<T>(params object[] args) where T : ApiController
		{
			var config = new HttpConfiguration();

			var request = new HttpRequestMessage();
			request.SetRequestContext(new HttpRequestContext { IncludeErrorDetail = true });
			request.SetConfiguration(config);

			T controller = (T)Activator.CreateInstance(typeof(T), args);
			controller.Request = request;
			return controller;
		}
	}
}