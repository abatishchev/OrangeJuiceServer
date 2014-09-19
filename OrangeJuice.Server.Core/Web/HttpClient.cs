using System;
using System.Threading.Tasks;

using SystemHttpClient  = System.Net.Http.HttpClient;

namespace OrangeJuice.Server.Web
{
	public class HttpClient : IHttpClient
	{
		private readonly SystemHttpClient _client = new SystemHttpClient();

		public virtual Task<string> GetStringAsync(Uri url)
		{
			return _client.GetStringAsync(url);
		}
	}
}