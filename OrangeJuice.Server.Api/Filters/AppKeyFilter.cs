using System;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Filters
{
	internal sealed class AppKeyFilter : System.Web.Http.Filters.ActionFilterAttribute
	{
		private const string AppKeySegmentName = "appKey";

		public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext context)
		{
			string appKey = GetAppKey(context.Request.RequestUri.Query);

			Guid guid;
			if (!Guid.TryParse(appKey, out guid) || guid != AppKey.Version0)
			{
				var response = context.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, "You can't use the API without application key");
				throw new System.Web.Http.HttpResponseException(response);
			}
		}

		private static string GetAppKey(string query)
		{
			return System.Web.HttpUtility.ParseQueryString(query).Get(AppKeySegmentName);
		}
	}
}