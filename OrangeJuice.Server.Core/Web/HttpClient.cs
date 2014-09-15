using System;
using System.IO;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Web
{
	public sealed class HttpClient : IHttpClient
	{
		public Task<Stream> GetStreamAsync(Uri url)
		{
			using (var client = new System.Net.Http.HttpClient
			{
				Timeout = TimeSpan.FromMinutes(1)
			})
			{

				return client.GetStreamAsync(url);
			}
		}
	}
}