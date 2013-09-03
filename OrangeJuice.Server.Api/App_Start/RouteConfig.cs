using System.Web.Http;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	internal class RouteConfig
	{
		public static void RegisterRoutes(HttpRouteCollection routes)
		{
			routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { controller = "Home", id = RouteParameter.Optional });
		}
	}
}