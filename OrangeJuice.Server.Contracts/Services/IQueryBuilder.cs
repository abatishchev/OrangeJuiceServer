using System.Collections.Generic;

namespace OrangeJuice.Server.Services
{
	public interface IQueryBuilder
	{
		string BuildUrl(IDictionary<string, string> args);
	}
}