using System.Collections.Generic;
using System.Collections.Specialized;

namespace OrangeJuice.Server.Services
{
	public interface IArgumentBuilder
	{
		NameValueCollection BuildArgs(IDictionary<string, string> args);
	}
}