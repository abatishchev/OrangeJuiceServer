using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Services
{
	public interface IAwsClient
	{
		Task<ICollection<XElement>> GetItems(IDictionary<string, string> args);
	}
}