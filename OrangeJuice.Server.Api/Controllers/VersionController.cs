using System;
using System.Net;
using System.Net.Http;
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

		public HttpResponseMessage GetVersion()
		{
			return Request.CreateResponse(HttpStatusCode.OK, _apiVersion);
		}
	}
}