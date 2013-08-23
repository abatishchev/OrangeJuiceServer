using System.Xml.Linq;

namespace OrangeJuice.Server.Api.Models
{
	public interface IGroceryDescriptionFactory
	{
		GroceryDescription Create(XElement element);
	}
}