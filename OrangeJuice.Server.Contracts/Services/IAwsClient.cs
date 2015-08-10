using System.Threading.Tasks;
using System.Xml.Linq;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Services
{
	public interface IAwsClient
	{
		Task<XElement[]> GetItems(AwsProductSearchCriteria searchCriteria);
	}
}