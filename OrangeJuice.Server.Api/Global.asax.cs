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

			RouteConfig.RegisterRoutes(GlobalConfiguration.Configuration.Routes);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			FilterConfig.RegisterFilters(GlobalFilters.Filters);
			FilterConfig.RegisterFilters(GlobalConfiguration.Configuration.Filters);

			WebApiConfig.Configure(GlobalConfiguration.Configuration);

			UnityConfig.InitializeContainer();
		}
	}
}