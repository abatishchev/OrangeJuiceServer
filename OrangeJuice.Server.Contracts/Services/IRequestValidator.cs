using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IRequestValidator
	{
		bool IsValid(XElement items);
	}
}