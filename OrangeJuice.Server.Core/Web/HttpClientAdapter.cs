using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Web
{
	public class HttpClientAdapter : IHttpClient
	{
		#region Fields

		private readonly HttpClient _httpClient;

		#endregion

		#region Ctor
		public HttpClientAdapter(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		#endregion

		#region IHttpClient members
		public Task<string> GetStringAsync(Uri url)
		{
			return _httpClient.GetStringAsync(url);
		}
		#endregion
	}
}