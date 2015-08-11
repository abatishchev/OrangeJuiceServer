using System;
using System.Web.Http.ExceptionHandling;
using Elmah.Contrib.WebApi;

namespace OrangeJuice.Server.Api
{
	public sealed class ElmahAggregateExceptionLogger : ElmahExceptionLogger
	{
		public override void Log(ExceptionLoggerContext context)
		{
			var aggregateException = context.Exception as AggregateException;
			if (aggregateException != null)
			{
				context = new ExceptionLoggerContext(new ExceptionContext(aggregateException.GetBaseException(), context.CatchBlock));
			}
			base.Log(context);
		}
	}
}