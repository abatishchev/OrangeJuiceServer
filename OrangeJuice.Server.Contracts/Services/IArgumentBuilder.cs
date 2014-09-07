using System.Collections.Generic;

namespace OrangeJuice.Server.Services
{
	public interface IArgumentBuilder
	{
		IDictionary<string, string> BuildArgs(Data.ProductDescriptorSearchCriteria searchCriteria);
	}
}