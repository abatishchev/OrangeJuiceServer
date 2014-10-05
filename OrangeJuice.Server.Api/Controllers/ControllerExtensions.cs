using System.Net;
using System.Web.Http;
using System.Web.Http.Results;

namespace OrangeJuice.Server.Api.Controllers
{
	public static class ControllerExtensions
	{
		public static IHttpActionResult NoContent(this ApiController controller)
		{
			return new StatusCodeResult(HttpStatusCode.NoContent, controller);
		}
	}
}