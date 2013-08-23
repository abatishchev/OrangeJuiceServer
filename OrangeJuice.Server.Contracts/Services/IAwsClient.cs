using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IAwsClient
	{
		Task<IEnumerable<string>> ItemSearch(string title);

		Task<XElement> ItemLookup(string asin);
	}
}