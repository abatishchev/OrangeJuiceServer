using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Factory;

using Microsoft.Practices.Unity;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Security;

namespace OrangeJuice.Server.Api.Test.Integration
{
	internal static class HttpClientFactory
	{
		private static readonly IUnityContainer Container = ContainerConfig.CreateWebApiContainer();

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
			return new Uri(Container.Resolve<IConfigurationProvider>().GetValue("environment:Url"));
		}

		private static AuthToken _authToken;

		private static async Task<AuthToken> GetAccessToken()
		{
			if (_authToken != null)
				return _authToken;

			var jwtFactory = Container.Resolve<IFactory<string>>(typeof(JwtFactory).Name);
			string jwt = jwtFactory.Create();

			var googleTokenFactory = Container.Resolve<IFactory<Task<AuthToken>, string>>(typeof(GoogleAuthTokenFactory).Name);
			AuthToken authorizationToken = await googleTokenFactory.Create(jwt);

			var bearerTokenFactory = Container.Resolve<IFactory<Task<AuthToken>, AuthToken>>(typeof(AuthTokenFactory).Name);
			_authToken = await bearerTokenFactory.Create(authorizationToken);
			return _authToken;
		}
	}
}