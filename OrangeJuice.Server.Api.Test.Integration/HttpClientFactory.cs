using System;
using System.Net.Http;

using OrangeJuice.Server.Api.Handlers;

namespace OrangeJuice.Server.Api.Test.Integration
{
    internal static class HttpClientFactory
    {
        private static readonly string Url = GetUrl();

        private static string GetUrl()
        {
            return System.Configuration.ConfigurationManager.AppSettings["test:EnvironmentUrl"];
        }

        public static HttpClient Create()
        {
            var client = new HttpClient { BaseAddress = new Uri(Url) };
            
            client.DefaultRequestHeaders.TryAddWithoutValidation(HeaderAppVersionHandler.HeaderName, AppVersion.Version0.ToString());

            return client;
        }
    }
}