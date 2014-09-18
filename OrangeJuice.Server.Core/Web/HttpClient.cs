using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Web
{
	public class HttpClient : IHttpClient
	{
		public virtual Task<string> GetStringAsync(Uri url)
		{
			var client = new System.Net.Http.HttpClient();
			return client.GetStringAsync(url);
		}
	}
}