using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Factory;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Models;

using SimpleInjector;

namespace OrangeJuice.Server.Api.Test.Integration
{
	internal static class HttpClientFactory
	{
		private static readonly Container _container = CreateiContainer();

		private static readonly Lazy<AuthToken> _authToken = new Lazy<AuthToken>(() => Task.Factory.StartNew(() => GetAccessToken()).Unwrap().Result);

		public static HttpClient Create()
		{
			var client = new HttpClient { BaseAddress = GetUrl(), Timeout = TimeSpan.FromSeconds(1) };

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.TryAddWithoutValidation("AppVer", AppVersion.Version0.ToString());
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken.Value.IdToken);

			return client;
		}

		private static Container CreateiContainer()
		{
			return ContainerConfig.CreateWebApiContainer();
		}

		private static Uri GetUrl()
		{
			return new Uri(_container.GetInstance<IConfigurationProvider>().GetValue("environment:Url"));
		}

		private static async Task<AuthToken> GetAccessToken()
		{
			var jwtFactory = _container.GetInstance<IFactory<string>>();
			string jwt = jwtFactory.Create();

			var googleTokenFactory = _container.GetInstance<IFactory<Task<AuthToken>, string>>();
			AuthToken authorizationToken = await googleTokenFactory.Create(jwt);

			var bearerTokenFactory = _container.GetInstance<IFactory<Task<AuthToken>, AuthToken>>();
			return await bearerTokenFactory.Create(authorizationToken);
		}
	}
}