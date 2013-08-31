using System.Web.Http.Filters;
using System.Web.Mvc;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	static class FilterConfig
	{
		/// <remarks>Mvc</remarks>
		public static void RegisterFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		/// <remarks>Web API</remarks>
		public static void RegisterFilters(HttpFilterCollection filters)
		{
			filters.Add(new Elmah.Contrib.WebApi.ElmahHandleErrorApiAttribute());
			filters.Add(new Filters.ModelValidationFilterAttribute());
		}
	}
}