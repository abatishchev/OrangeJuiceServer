using System.Web.Http;

namespace OrangeJuice.Server.Api
{
	public class GlobalApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			RouteConfig.RegisterRoutes(GlobalConfiguration.Configuration.Routes);
			FilterConfig.RegisterFilters(GlobalConfiguration.Configuration.Filters);

			var container = UnityConfig.InitializeContainer();
			WebApiConfig.Configure(GlobalConfiguration.Configuration, container);
		}
	}
}