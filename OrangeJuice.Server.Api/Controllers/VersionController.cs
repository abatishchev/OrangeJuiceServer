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
		private readonly IApiVersionFactory _apiVersionFactory;

		public VersionController(IApiVersionFactory apiVersionFactory)
		{
			if (apiVersionFactory == null)
				throw new ArgumentNullException("apiVersionFactory");
			_apiVersionFactory = apiVersionFactory;
		}

		public async Task<HttpResponseMessage> GetVersion()
		{
			ApiVersion version = await _apiVersionFactory.Create();
			return Request.CreateResponse(HttpStatusCode.OK, version);
		}
	}
}