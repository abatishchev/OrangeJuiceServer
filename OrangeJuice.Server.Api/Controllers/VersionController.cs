using System;
using System.Web.Http;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class VersionController : ApiController
	{
		private readonly ApiVersion _apiVersion;

		public VersionController(ApiVersion apiVersion)
		{
			if (apiVersion == null)
				throw new ArgumentNullException("apiVersion");
			_apiVersion = apiVersion;
		}

		public IHttpActionResult GetVersion()
		{
			return Ok(_apiVersion);
		}
	}
}