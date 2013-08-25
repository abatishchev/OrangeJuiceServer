using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Data
{
	public interface IFoodDescriptionFactory
	{
		FoodDescription Create(Task<XElement> descriptionTask, Task<XElement> imageTask);
	}
}