using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Ab.Configuration;
using Ab.Factory;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Security;

using SimpleInjector;

namespace OrangeJuice.Server.Api.Test.Integration
{
	internal static class HttpClientFactory
	{
		private static readonly Container _container = CreateContainer();

		private static readonly Lazy<AuthToken> _authToken = new Lazy<AuthToken>(() => Task.Factory.StartNew(() => GetAccessToken()).Unwrap().Result);

		public static HttpClient Create()
		{
			var client = new HttpClient { BaseAddress = GetUrl() };

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(String.Format("application/vnd.orangejuice.v{0}+json", AppVersion.Version0)));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken.Value.IdToken);

			return client;
		}

		private static Container CreateContainer()
		{
			return ContainerConfig.CreateWebApiContainer();
		}

		private static Uri GetUrl()
		{
			return new Uri(_container.GetInstance<IConfigurationProvider>().GetValue("environment:Url"));
		}

		private static async Task<AuthToken> GetAccessToken()
		{
			var jwtFactory = _container.GetInstance<IFactory<Jwt>>();
			var jwt = jwtFactory.Create();

			var googleTokenFactory = _container.GetInstance<IFactory<Task<AuthToken>, string>>();
			AuthToken authorizationToken = await googleTokenFactory.Create(jwt.Value);

			var bearerTokenFactory = _container.GetInstance<IFactory<Task<AuthToken>, AuthToken>>();
			return await bearerTokenFactory.Create(authorizationToken);
		}
	}
}