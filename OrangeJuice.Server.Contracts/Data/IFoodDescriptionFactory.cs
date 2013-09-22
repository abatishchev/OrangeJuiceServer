using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public interface IFoodDescriptionFactory
	{
		string GetId(XElement item);

		Task<FoodDescription> Create(string id, Task<XElement> attributesTask, Task<XElement> imagesTask);
	}
}