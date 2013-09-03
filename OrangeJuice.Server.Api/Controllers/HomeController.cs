using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Controllers
{
	// TODO: split into ApiVersionController?
	public sealed class HomeController : ApiController
	{
		private readonly IApiInfoFactory _apiInfoFactory;

		public HomeController(IApiInfoFactory apiInfoFactory)
		{
			if (apiInfoFactory == null)
				throw new ArgumentNullException("apiInfoFactory");
			_apiInfoFactory = apiInfoFactory;
		}

		public async Task<HttpResponseMessage> GetVersion()
		{
			return Request.RequestUri.ParseQueryString().Cast<string>().Contains("version") ?
				Request.CreateResponse(HttpStatusCode.OK, _apiInfoFactory.Create()) :
				Request.CreateResponse(HttpStatusCode.Forbidden);
		}
	}
}