using System;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;

using Elmah;

namespace OrangeJuice.Server.Api.Logging
{
	public class ElmahExceptionLogger : ExceptionLogger
	{
		#region Fields
		private const string HttpContextBaseKey = "MS_HttpContext";
		#endregion

		#region ExceptionLogger members
		public override void Log(ExceptionLoggerContext context)
		{
			// Retrieve the current HttpContext instance for this request.
			HttpContext httpContext = GetHttpContext(context.Request);

			if (httpContext == null)
				return;

			// Wrap the exception in an HttpUnhandledException so that ELMAH can capture the original error page.
			Exception exceptionToRaise = new HttpUnhandledException(context.Exception.Message, context.Exception);

			// Send the exception to ELMAH (for logging, mailing, filtering, etc.).
			ErrorSignal signal = ErrorSignal.FromContext(httpContext);
			signal.Raise(exceptionToRaise, httpContext);
		}
		#endregion

		#region Methods
		private static HttpContext GetHttpContext(HttpRequestMessage request)
		{
			if (request == null)
				return null;

			object value;
			if (!request.Properties.TryGetValue(HttpContextBaseKey, out value))
				return null;

			HttpContextBase context = value as HttpContextBase;
			if (context == null)
				return null;

			return context.ApplicationInstance.Context;
		}
		#endregion
	}
}