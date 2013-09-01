using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Web
{
	public interface IDocumentLoader
	{
		Task<XDocument> Load(string url);
	}
}