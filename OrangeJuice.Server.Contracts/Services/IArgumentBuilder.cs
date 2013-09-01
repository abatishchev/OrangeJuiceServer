using System.Collections.Generic;

namespace OrangeJuice.Server.Services
{
	public interface IArgumentBuilder
	{
		IDictionary<string, string> BuildArgs(IDictionary<string, string> args);
	}
}