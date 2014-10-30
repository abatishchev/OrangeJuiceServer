using System;
using System.IdentityModel.Tokens;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Security
{
	public sealed class TokenValidationParametersFactory : Factory.IFactory<TokenValidationParameters>
	{
		private readonly AuthOptions _authOptions;

		public TokenValidationParametersFactory(AuthOptions authOptions)
		{
			_authOptions = authOptions;
		}

		public TokenValidationParameters Create()
		{
			byte[] symmetricKey = Convert.FromBase64String(_authOptions.CertificateKey.Replace('-', '+').Replace('_', '/'));
			return new TokenValidationParameters
			{
				IssuerSigningKey = new InMemorySymmetricSecurityKey(symmetricKey),
				RequireExpirationTime = true,
				ValidAudience = _authOptions.Audience,
				ValidIssuer = _authOptions.Issuer
			};
		}
	}
}