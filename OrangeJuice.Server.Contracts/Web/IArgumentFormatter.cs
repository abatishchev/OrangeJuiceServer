using System.Collections.Generic;
using System.Collections.Specialized;

namespace OrangeJuice.Server.Web
{
	public interface IArgumentFormatter
	{
		NameValueCollection FormatArgs(IDictionary<string, string> args);
	}
}