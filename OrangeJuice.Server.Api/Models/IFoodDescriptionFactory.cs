using System.Xml.Linq;

namespace OrangeJuice.Server.Api.Models
{
	public interface IFoodDescriptionFactory
	{
		FoodDescription Create(XElement element);
	}
}