using System.Web.Http;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class HomeController : ApiController
	{
		private readonly ApiVersion _apiVersion;

		public HomeController(ApiVersion apiVersion)
		{
			_apiVersion = apiVersion;
		}

		[Route("api")]
		public IHttpActionResult Get()
		{
			return NotFound();
		}

		[Route("api/version")]
		public IHttpActionResult GetVersion()
		{
			return Ok(_apiVersion);
		}
	}
}