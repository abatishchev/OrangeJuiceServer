using System.Net;
using System.Web.Http;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class HomeController : ApiController
	{
		public IHttpActionResult Get()
		{
			return StatusCode(HttpStatusCode.Forbidden);
		}
	}
}