using System;
using System.Net.Http;

namespace OrangeJuice.Server.Api.Filters
{
	internal abstract class QueryFilterBase : System.Web.Http.Filters.ActionFilterAttribute
	{
		protected abstract string QuerySegmentName { get; }

		public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext context)
		{
			string appKey = GetAppKey(context.Request.RequestUri.Query);

			Guid guid;
			if (!Guid.TryParse(appKey, out guid) || guid != AppKey.Version0)
			{
				var response = context.Request.CreateErrorResponse(System.Net.HttpStatusCode.Unauthorized, "You can't use the API without the key");
				throw new System.Web.Http.HttpResponseException(response);
			}
		}

		private string GetAppKey(string query)
		{
			return System.Web.HttpUtility.ParseQueryString(query).Get(QuerySegmentName);
		}
	}
}