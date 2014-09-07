using System.Web.Http;

namespace OrangeJuice.Server.Api
{
	public class GlobalApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var container = ContainerConfig.CreateContainer();

			GlobalConfiguration.Configure(c => WebApiConfig.Configure(c, container));
		}
	}
}