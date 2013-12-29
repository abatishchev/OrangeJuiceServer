using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public interface IFoodDescriptionFactory
	{
		FoodDescription Create(string id, XElement attributesElement, XElement imagesElement);
	}
}