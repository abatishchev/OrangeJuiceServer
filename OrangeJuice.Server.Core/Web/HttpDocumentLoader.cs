using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OrangeJuice.Server.Web
{
	public sealed class HttpDocumentLoader : IDocumentLoader
	{
		private readonly HttpClient _httpClient = new HttpClient();

		public async Task<XDocument> Load(string url)
		{
			if (String.IsNullOrEmpty(url))
				throw new ArgumentNullException("url");

			using (Stream stream = await _httpClient.GetStreamAsync(url))
			{
				return XDocument.Load(stream);
			}
		}
	}
}