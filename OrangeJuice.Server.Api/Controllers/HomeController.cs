using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class HomeController : ApiController
	{
		public HttpResponseMessage Get()
		{
			return new HttpResponseMessage(HttpStatusCode.Forbidden);
		}
	}
}