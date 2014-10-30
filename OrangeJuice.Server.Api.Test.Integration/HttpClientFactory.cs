using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace OrangeJuice.Server.Api.Test.Integration
{
	internal static class HttpClientFactory
	{
		private static readonly string Url = GetUrl();

		private static string GetUrl()
		{
			return System.Configuration.ConfigurationManager.AppSettings["environment:Url"];
		}

		public static HttpClient Create()
		{
			var client = new HttpClient { BaseAddress = new Uri(Url) };

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.TryAddWithoutValidation("AppVer", AppVersion.Version0.ToString());

			return client;
		}
	}
}