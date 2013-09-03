using System.Web.Http;

namespace OrangeJuice.Server.Api
{
	public class GlobalApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var container = UnityConfig.InitializeContainer();

			RouteConfig.RegisterRoutes(GlobalConfiguration.Configuration.Routes);

			FilterConfig.RegisterFilters(GlobalConfiguration.Configuration.Filters);

			WebApiConfig.Configure(GlobalConfiguration.Configuration, container);
		}
	}
}