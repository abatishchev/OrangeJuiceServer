using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IIdSelector
	{
		string GetId(XElement arg);
	}
}