using System.Collections.Generic;

namespace OrangeJuice.Server.Web
{
	public interface IQueryBuilder
	{
		string BuildQuery(IEnumerable<KeyValuePair<string, string>> args);

		string SignQuery(string query, string signature);
	}
}