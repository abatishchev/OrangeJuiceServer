using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public interface IFoodDescriptorFactory
	{
		FoodDescriptor Create(XElement element);
	}
}