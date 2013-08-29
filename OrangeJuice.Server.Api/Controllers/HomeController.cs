using System;
using System.Web.Mvc;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class HomeController : Controller
	{
		private readonly ApiInfoFactory _apiInfoFactory;

		public HomeController(ApiInfoFactory apiInfoFactory)
		{
			_apiInfoFactory = apiInfoFactory;
		}

		public HttpStatusCodeResult Index()
		{
			return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
		}

		public JsonDotNetResult Version()
		{
			return new JsonDotNetResult(_apiInfoFactory.Create());
		}
	}
}