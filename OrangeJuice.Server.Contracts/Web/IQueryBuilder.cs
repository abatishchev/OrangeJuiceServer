using System.Collections.Generic;

namespace OrangeJuice.Server.Web
{
	public interface IQueryBuilder
	{
		string BuildQuery(IDictionary<string, string> args);
	}
}