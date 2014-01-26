using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IAwsProvider
	{
		Task<IEnumerable<XElement>> SearchItems(string title);

		Task<IEnumerable<XElement>> ItemLookup(string barcode, string barcodeType);
	}
}