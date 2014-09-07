using System;
using System.IO;
using System.Threading.Tasks;




namespace OrangeJuice.Server.Web

{

	public sealed class HttpClient : IHttpClient
	{

		public Task<Stream> ParseQueryString(Uri url)

		{

			using (var client = new HttpClient())
			{
				return client.GetStreamAsync(url);
			}
		}

	}

}