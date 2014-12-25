using System.Web.Http;

namespace OrangeJuice.Server.Controllers
{
	public interface IVersionController
	{
		IHttpActionResult GetVersion();
	}
}