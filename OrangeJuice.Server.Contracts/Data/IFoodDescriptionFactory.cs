using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public interface IFoodDescriptionFactory
	{
		string GetId(XElement item);

		FoodDescription Create(XElement attributesElement, XElement imagesElement);
	}
}