using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Web
{
	public sealed class HttpDocumentLoader : IDocumentLoader
	{
		public async Task<XDocument> Load(string url)
		{
			using (HttpClient client = HttpClientFactory.Create())
			using (Stream stream = await client.GetStreamAsync(url))
			{
				return XDocument.Load(stream);
			}
		}
	}
}