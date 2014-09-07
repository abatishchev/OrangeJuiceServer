using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IItemSelector
	{
		IEnumerable<XElement> SelectItems(Stream stream);
	}
}