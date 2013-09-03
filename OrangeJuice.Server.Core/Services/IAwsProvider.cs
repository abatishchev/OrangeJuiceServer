using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IAwsProvider
	{
		Task<IEnumerable<XElement>> SearchItem(string title);

		Task<XElement> LookupAttributes(string id);

		Task<XElement> LookupImages(string id);
	}
}