using System.Web.Http;

namespace OrangeJuice.Server.Api
{
	public class GlobalApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var container = ContaineConfig.CreateContainer();

			GlobalConfiguration.Configure(c => WebApiConfig.Configure(c, container));

			RouteConfig.ConfigureRoutes(GlobalConfiguration.Configuration.Routes);
		}
	}
}