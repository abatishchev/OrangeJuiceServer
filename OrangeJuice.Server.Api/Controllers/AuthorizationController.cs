﻿using System.Threading.Tasks;
using System.Web.Http;

using Factory;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Api.Controllers
{
	public class AuthorizationController : ApiController
	{
		private readonly IFactory<string> _jwtFactory;
		private readonly IFactory<Task<AuthToken>, string> _googleTokenFactory;
		private readonly IFactory<Task<AuthToken>, AuthToken> _bearerTokenFactory;

		public AuthorizationController(IFactory<string> jwtFactory, IFactory<Task<AuthToken>, string> googleTokenFactory, IFactory<Task<AuthToken>, AuthToken> bearerTokenFactory)
		{
			_jwtFactory = jwtFactory;
			_googleTokenFactory = googleTokenFactory;
			_bearerTokenFactory = bearerTokenFactory;
		}

		[Route("api/auth/token")]
		public async Task<IHttpActionResult> Get()
		{
			string jwt = _jwtFactory.Create();
			AuthToken authorizationToken = await _googleTokenFactory.Create(jwt);
			AuthToken authToken = await _bearerTokenFactory.Create(authorizationToken);

			return Ok(authToken);
		}
	}
}