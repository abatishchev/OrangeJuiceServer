using System.Collections.Generic;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IItemSelector
	{
		XElement GetItem(XDocument doc);

		IEnumerable<XElement> GetItems(XDocument doc);
	}
}