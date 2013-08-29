using System;
using System.Web.Mvc;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class HomeController : Controller
	{
		private readonly IApiInfoFactory _apiInfoFactory;

		// TODO: remove
		public HomeController()
			: this(new ApiInfoFactory())
		{
		}

		public HomeController(IApiInfoFactory apiInfoFactory)
		{
			if (apiInfoFactory == null)
				throw new ArgumentNullException("apiInfoFactory");
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