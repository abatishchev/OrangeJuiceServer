using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IAwsProvider
	{
		Task<ICollection<XElement>> SearchItems(string title);

		Task<ICollection<XElement>> LookupAttributes(IEnumerable<string> ids);

		Task<ICollection<XElement>> LookupImages(IEnumerable<string> ids);
	}
}