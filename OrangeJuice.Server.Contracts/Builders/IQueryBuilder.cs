using System.Collections.Generic;

namespace OrangeJuice.Server.Builders
{
	public interface IQueryBuilder
	{
		string BuildQuery(IDictionary<string, string> dic);
	}
}