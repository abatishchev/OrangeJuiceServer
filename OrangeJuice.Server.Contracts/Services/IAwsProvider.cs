using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IAwsProvider
	{
		Task<IEnumerable<XElement>> SearchItem(string title);

		Task<IEnumerable<XElement>> LookupAttributes(IEnumerable<string> ids);

		Task<IEnumerable<XElement>> LookupImages(IEnumerable<string> ids);
	}
}