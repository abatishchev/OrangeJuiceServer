using System;
using System.Web.Mvc;

using Newtonsoft.Json;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class HomeController : Controller
	{
		private readonly Lazy<ApiInfo> _apiInfo = new Lazy<ApiInfo>(ApiInfo.Create);

		public HttpStatusCodeResult Index()
		{
			return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
		}

		public JsonDotNetResult Version()
		{
			return new JsonDotNetResult(_apiInfo.Value);
		}
	}
}