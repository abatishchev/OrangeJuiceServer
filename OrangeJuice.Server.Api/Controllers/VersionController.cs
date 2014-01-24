using System.Web.Http;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class VersionController : ApiController
	{
		private readonly ApiVersion _apiVersion;

		public VersionController(ApiVersion apiVersion)
		{
			_apiVersion = apiVersion;
		}

		/// <url>PUT /api/version</url>
		public IHttpActionResult GetVersion()
		{
			return Ok(_apiVersion);
		}
	}
}