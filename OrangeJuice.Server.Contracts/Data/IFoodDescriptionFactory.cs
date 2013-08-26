using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public interface IFoodDescriptionFactory
	{
		Task<FoodDescription> Create(string id, Task<XElement> attributesTask, Task<XElement> imagesTask);
	}
}