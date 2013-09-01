using System.Collections.Generic;

namespace OrangeJuice.Server.Builders
{
	public interface IArgumentBuilder
	{
		IDictionary<string, string> BuildArgs(IDictionary<string, string> args);
	}
}