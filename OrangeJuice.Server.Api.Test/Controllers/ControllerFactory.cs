using System;
using System.Net.Http;
using System.Web.Http;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	internal static class ControllerFactory
	{
		public static T Create<T>(params object[] args) where T : ApiController
		{
			var request = new HttpRequestMessage();
			request.SetConfiguration(new HttpConfiguration());

			T controller = (T)Activator.CreateInstance(typeof(T), args);
			controller.Request = request;
			controller.RequestContext.IncludeErrorDetail = true;
			return controller;
		}
	}
}