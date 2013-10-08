using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public interface IFoodDescriptionFactory
	{
		string GetId(XElement element);

		FoodDescription Create(XElement attributesElement, XElement imagesElement);
	}
}