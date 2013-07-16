using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace OrangeJuice.Server.Api
{
	public class GlobalApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

		    UnityConfig.Initialize();
		}
	}
}