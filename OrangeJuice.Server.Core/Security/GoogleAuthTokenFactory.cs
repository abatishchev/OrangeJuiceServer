﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

using Ab.Factory;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Security
{
	public sealed class GoogleAuthTokenFactory : IFactory<Task<AuthToken>, string>
	{
		private readonly IFactory<Jwt> _jwtFactory;

		public GoogleAuthTokenFactory(IFactory<Jwt> jwtFactory)
		{
			_jwtFactory = jwtFactory;
		}

		public async Task<AuthToken> Create(string authorizationToken)
		{
			var jwt = _jwtFactory.Create();

			var dic = new Dictionary<string, string>
			{
				{ "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
				{ "assertion", jwt.Value }
			};
			var content = new FormUrlEncodedContent(dic);

			var httpClient = new HttpClient { BaseAddress = new Uri("https://accounts.google.com") };
			var response = await httpClient.PostAsync("/o/oauth2/token", content);
			response.EnsureSuccessStatusCode();

			var formatters = new[]
			{
				new JsonMediaTypeFormatter
				{
					SerializerSettings =
					{
						ContractResolver = new UnderscoreMappingResolver()
					}
				}
			};
			return await response.Content.ReadAsAsync<AuthToken>(formatters);
		}
	}
}