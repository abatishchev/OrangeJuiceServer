using System.Web.Http;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	internal static class RouteConfig
	{
		public static void RegisterRoutes(HttpRouteCollection routes)
		{
			// TODO: why ignored?
			routes.MapHttpRoute(
				name: "WithAction",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { controller = "Home", id = RouteParameter.Optional }
			);

			routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { controller = "Home", id = RouteParameter.Optional });
		}
	}
}