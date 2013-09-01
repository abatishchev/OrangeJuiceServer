using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IItemProvider
	{
		XElement GetItems(XDocument doc);
	}
}