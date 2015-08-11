using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace OrangeJuice.Server.Api
{
	public sealed class ElmahAggregateExceptionLogger : Elmah.Contrib.WebApi.ElmahExceptionLogger
	{
		public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
		{
			var aggregateException = context.Exception as AggregateException;
			if (aggregateException != null)
			{
				context = new ExceptionLoggerContext(new ExceptionContext(aggregateException.GetBaseException(), context.CatchBlock));
			}
			return base.LogAsync(context, cancellationToken);
		}
	}
}