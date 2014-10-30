using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Security
{
    public sealed class AuthTokenFactory : Factory.IFactory<Task<AuthToken>, AuthToken>
	{
		private readonly AuthOptions _authOptions;

		public AuthTokenFactory(AuthOptions authOptions)
		{
			_authOptions = authOptions;
		}

		public async Task<AuthToken> Create(AuthToken authorizationToken)
		{
			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

			var request = new
			{
				client_id = _authOptions.Audience,
				access_token = authorizationToken.AccessToken,
				connection = "google-oauth2",
				scope = "openid"
			};

			var response = await httpClient.PostAsync("https://orangejuice.auth0.com/oauth/access_token", request, new JsonMediaTypeFormatter());
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsAsync<AuthToken>(UnderscoreMappingResolver.Formatters);
		}
	}
}