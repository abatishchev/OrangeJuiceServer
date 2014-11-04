using System.Web.Http;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class HomeController : ApiController
	{
		private readonly ApiVersion _apiVersion;

		public HomeController(ApiVersion apiVersion)
		{
			_apiVersion = apiVersion;
		}

		[Route("api/version")]
		public IHttpActionResult GetVersion()
		{
			return Ok(_apiVersion);
		}
	}
}