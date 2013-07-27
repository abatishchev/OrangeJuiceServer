using System;
using System.Text;
using System.Diagnostics;

namespace OrangeJuice.Server.Diagnostics
{
	/// <summary>
	/// Provides extension methods for the TraceSource class
	/// </summary>
	/// <see cref="System.Diagnostics.TraceSource" />
	public static class TraceSourceExtensions
	{
		/// <summary>
		/// Traces exception details to the specified trace source
		/// </summary>
		/// <param name="traceSource">The trace source to which the exception is to be written</param>
		/// /// <param name="exception">The exception to be traced</param>
		/// <param name="id">To be passed to the id parameter of the TraceEvent method of the TraceSource</param>
		/// <param name="message">The message (format string) associated with the exception</param>
		/// <param name="args">Arguments used to populate the message format string</param>
		public static void TraceException(this TraceSource traceSource, Exception exception, int id, string message, params object[] args)
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat(message, args);
			builder.AppendLine(":");
			builder.Append(exception);
			traceSource.TraceEvent(TraceEventType.Error, id, builder.ToString());
		}
	}
}