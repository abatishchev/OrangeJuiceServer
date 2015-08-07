using System.Collections.Generic;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public interface IArgumentBuilder
	{
		IDictionary<string, string> BuildArgs(AwsProductSearchCriteria searchCriteria);
	}
}