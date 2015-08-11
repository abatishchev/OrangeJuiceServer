using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IItemSelector
	{
		XElement[] SelectItems(string xml);
	}
}