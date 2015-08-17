using System.Web.Http;

using Ab.Configuration;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;

using Owin;
using SimpleInjector;

namespace OrangeJuice.Server.Api
{
	internal static class OwinConfig
	{
		public static void Configure(IAppBuilder app, HttpConfiguration config, Container container)
		{
			app.UseWebApi(config);

			ConfigureOAuth(app, container);
		}

		private static void ConfigureOAuth(IAppBuilder app, Container container)
		{
			AuthOptions authOptions = container.GetInstance<AuthOptions>();

			app.UseJwtBearerAuthentication(
				new JwtBearerAuthenticationOptions
				{
					AuthenticationMode = AuthenticationMode.Active,
					AllowedAudiences = new[] { authOptions.Audience },
					IssuerSecurityTokenProviders = new[]
					{
						new SymmetricKeyIssuerSecurityTokenProvider(authOptions.Issuer, TextEncodings.Base64Url.Decode(authOptions.CertificateKey))
					}
				});
		}
	}
}