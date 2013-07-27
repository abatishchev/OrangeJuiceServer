using System.Web.Http.Filters;
using System.Web.Mvc;

using OrangeJuice.Server.Api.Filters;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	static class FilterConfig
	{
		public static void RegisterFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterFilters(HttpFilterCollection filters)
		{
			filters.Add(new UnhandledExceptionFilterAttribute(typeof(System.Data.DataException)));
		}
	}
}