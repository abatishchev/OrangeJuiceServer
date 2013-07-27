using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace OrangeJuice.Server.Api.Filters
{
	public class UnhandledExceptionFilterAttribute : ExceptionFilterAttribute
	{
		private readonly Type _exceptionType;
		private readonly bool _includeErrorDetail;

		public UnhandledExceptionFilterAttribute(Type exceptionType, bool includeErrorDetail = false)
		{
			if (exceptionType == null)
				throw new ArgumentNullException("exceptionType");
			_exceptionType = exceptionType;
			_includeErrorDetail = includeErrorDetail;
		}

		public override void OnException(HttpActionExecutedContext context)
		{
			if (context == null)
				throw new ArgumentNullException("context");

			if (_exceptionType.IsInstanceOfType(context.Exception))
			{
				context.Response = context.Request.CreateErrorResponse(
					HttpStatusCode.InternalServerError,
					new HttpError(context.Exception, _includeErrorDetail));
			}
		}
	}
}