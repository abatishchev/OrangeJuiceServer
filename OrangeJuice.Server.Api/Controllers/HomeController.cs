using System.Web.Mvc;

namespace OrangeJuice.Server.Api.Controllers
{
	public sealed class HomeController : Controller
	{
		public HttpStatusCodeResult Index()
		{
			return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
		}
	}
}