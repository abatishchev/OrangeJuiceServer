using System.Web.Http;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class VersionController : ApiController
	{
		private readonly ApiVersion _apiVersion;

		public VersionController(ApiVersion apiVersion)
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