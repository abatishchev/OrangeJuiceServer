using System;
using System.Net.Http;
using System.Web.Http;

using Drum;

namespace OrangeJuice.Server.Api.Test.Controllers
{
	internal static class ControllerFactory<T> where T : ApiController
	{
		public static T Create(params object[] args)
		{
			T controller = (T)Activator.CreateInstance(typeof(T), args);
			controller.Request = CreateRequest();
			//controller.RequestContext.IncludeErrorDetail = true;
			return controller;
		}

		private static HttpRequestMessage CreateRequest(HttpConfiguration config = null)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://example.com/api"));
			request.SetConfiguration(config ?? new HttpConfiguration());
			request.ShouldIncludeErrorDetail();
			return request;
		}

		public static UriMaker<T> CreateUriMaker()
		{
			var config = new HttpConfiguration();
			var uriMakerContext = config.MapHttpAttributeRoutesAndUseUriMaker();
			config.EnsureInitialized();

			var request = CreateRequest(config);

			return new UriMaker<T>(uriMakerContext, request);
		}
	}
}