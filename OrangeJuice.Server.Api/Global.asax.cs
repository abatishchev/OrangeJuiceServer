using System.Web.Http;

using Microsoft.Owin;

using Owin;

[assembly: OwinStartup(typeof(OrangeJuice.Server.Api.Startup))]
namespace OrangeJuice.Server.Api
{
	public class GlobalApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			var container = ContainerConfig.CreateWebApiContainer(registerControllers: true);

			GlobalConfiguration.Configure(c => WebApiConfig.Configure(c, container));
		}
	}

	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var config = new HttpConfiguration();
			var container = ContainerConfig.CreateOwinContainer();

			OwinConfig.Configure(app, config, container);
		}
	}
}