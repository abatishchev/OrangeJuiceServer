using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class VersionController : ApiController
	{
		private readonly ApiVersion _version;

		public VersionController(ApiVersion version)
		{
			if (version == null)
				throw new ArgumentNullException("version");
			_version = version;
		}

		public async Task<HttpResponseMessage> GetVersion()
		{
			return Request.CreateResponse(HttpStatusCode.OK, _version);
		}
	}
}