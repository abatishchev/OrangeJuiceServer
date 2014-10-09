using System;
using System.Collections;
using System.Diagnostics;

using Elmah;

namespace OrangeJuice.Server.Data.Logging
{
	public class TraceErrorLog : ErrorLog
	{
		private static readonly TraceSource _traceSource = new TraceSource("TraceErrorLog");

		#region ErrorLog members
		public override string Log(Error error)
		{
			string id = Guid.NewGuid().ToString();
			
			_traceSource.TraceEvent(TraceEventType.Error, 1, "Error id={0}, message={1}, exception={2}", id, error.Message, error.Exception);

			return id;
		}

		public override ErrorLogEntry GetError(string id)
		{
			throw new NotSupportedException();
		}

		public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
		{
			throw new NotSupportedException();
		}
		#endregion
	}
}