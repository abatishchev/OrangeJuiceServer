using System.Collections.Generic;

namespace OrangeJuice.Server.Web
{
	public interface IArgumentFormatter
	{
		string FormatArgs(IDictionary<string, string> args);
	}
}