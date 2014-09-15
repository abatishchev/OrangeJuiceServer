using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Web
{
	public sealed class HttpClient : IHttpClient
	{
		public Task<string> GetStringAsync(Uri url)
		{
			var client = new System.Net.Http.HttpClient();
			return client.GetStringAsync(url);
		}
	}
}