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
		private static readonly Container Container = ContainerConfig.CreateWebApiContainer();

		public static async Task<HttpClient> Create()
		{
			var client = new HttpClient { BaseAddress = GetUrl() };

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.TryAddWithoutValidation("AppVer", AppVersion.Version0.ToString());
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (await GetAccessToken()).IdToken);

			return client;
		}

		private static Uri GetUrl()
		{
			return new Uri(Container.GetInstance<IConfigurationProvider>().GetValue("environment:Url"));
		}

		private static AuthToken _authToken;

		private static async Task<AuthToken> GetAccessToken()
		{
			if (_authToken != null)
				return _authToken;

			var jwtFactory = Container.GetInstance<IFactory<string>>();
			string jwt = jwtFactory.Create();

			var googleTokenFactory = Container.GetInstance<IFactory<Task<AuthToken>, string>>();
			AuthToken authorizationToken = await googleTokenFactory.Create(jwt);

			var bearerTokenFactory = Container.GetInstance<IFactory<Task<AuthToken>, AuthToken>>();
			_authToken = await bearerTokenFactory.Create(authorizationToken);
			return _authToken;
		}
	}
}