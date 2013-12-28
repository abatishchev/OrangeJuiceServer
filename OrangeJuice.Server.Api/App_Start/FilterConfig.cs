﻿using System.Web.Http.Filters;

// ReSharper disable CheckNamespace
namespace OrangeJuice.Server.Api
{
	internal static class FilterConfig
	{
		public static void RegisterFilters(HttpFilterCollection filters)
		{
			filters.Add(new Elmah.Contrib.WebApi.ElmahHandleErrorApiAttribute());
		}
	}
}