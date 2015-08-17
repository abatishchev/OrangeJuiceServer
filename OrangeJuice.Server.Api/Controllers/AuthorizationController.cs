using System.Threading.Tasks;
using System.Web.Http;

using Ab.Factory;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Security;

namespace OrangeJuice.Server.Api.Controllers
{
	public class AuthorizationController : ApiController
	{
		private readonly IFactory<Jwt> _jwtFactory;
		private readonly IFactory<Task<AuthToken>, string> _googleTokenFactory;
		private readonly IFactory<Task<AuthToken>, AuthToken> _bearerTokenFactory;

		public AuthorizationController(IFactory<Jwt> jwtFactory, IFactory<Task<AuthToken>, string> googleTokenFactory, IFactory<Task<AuthToken>, AuthToken> bearerTokenFactory)
		{
			_jwtFactory = jwtFactory;
			_googleTokenFactory = googleTokenFactory;
			_bearerTokenFactory = bearerTokenFactory;
		}

		[Route("api/auth/token")]
		public async Task<IHttpActionResult> Get()
		{
			Jwt jwt = _jwtFactory.Create();
			AuthToken authorizationToken = await _googleTokenFactory.Create(jwt.Value);
			AuthToken authToken = await _bearerTokenFactory.Create(authorizationToken);

			return Ok(authToken);
		}
	}
}