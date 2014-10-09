using System.Collections;

using Elmah;

namespace OrangeJuice.Server.Data.Logging
{
	public class TraceErrorLog : ErrorLog
	{
		public override string Log(Error error)
		{
			throw new System.NotImplementedException();
		}

		public override ErrorLogEntry GetError(string id)
		{
			throw new System.NotImplementedException();
		}

		public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
		{
			throw new System.NotImplementedException();
		}
	}
}