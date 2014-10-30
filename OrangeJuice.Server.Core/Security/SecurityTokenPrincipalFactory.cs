using System.IdentityModel.Tokens;
using System.Security.Principal;

using Factory;

namespace OrangeJuice.Server.Security
{
	public sealed class SecurityTokenPrincipalFactory : IFactory<IPrincipal, string>
	{
		#region Fields
		private readonly ISecurityTokenValidator _tokenValidator;
		private readonly IFactory<TokenValidationParameters> _parametersFactory;
		#endregion

		#region Ctor
		public SecurityTokenPrincipalFactory(ISecurityTokenValidator tokenValidator, IFactory<TokenValidationParameters> parametersFactory)
		{
			_tokenValidator = tokenValidator;
			_parametersFactory = parametersFactory;
		}

		#endregion

		#region IFactory members
		public IPrincipal Create(string token)
		{
			SecurityToken securityToken;
			IPrincipal principal = _tokenValidator.ValidateToken(token, _parametersFactory.Create(), out securityToken);

			JwtSecurityToken jwtSecurityToken = (JwtSecurityToken)securityToken;

			return principal;
		}
		#endregion
	}
}