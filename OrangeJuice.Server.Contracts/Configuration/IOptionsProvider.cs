using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Configuration
{
	public interface IOptionsProvider<T>
	{
		Task<IEnumerable<T>> GetOptions();
	}
}