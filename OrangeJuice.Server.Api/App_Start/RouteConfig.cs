using System.Web.Http;

namespace OrangeJuice.Server.Api
{
	internal static class RouteConfig
	{
		public static void ConfigureRoutes(HttpRouteCollection routes)
		{
			routes.MapHttpRoute(
				name: "WithAction",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { controller = "Home", id = RouteParameter.Optional });

			routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { controller = "Home", id = RouteParameter.Optional });
		}

	}
}