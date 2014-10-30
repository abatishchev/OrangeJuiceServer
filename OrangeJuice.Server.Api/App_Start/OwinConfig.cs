using System.Web.Http;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Practices.Unity;

using OrangeJuice.Server.Configuration;

using Owin;

namespace OrangeJuice.Server.Api
{
	internal static class OwinConfig
	{
		public static void Configure(IAppBuilder app, HttpConfiguration config, IUnityContainer container)
		{
			app.UseWebApi(config);

			ConfigureOAuth(app, container);
		}

		private static void ConfigureOAuth(IAppBuilder app, IUnityContainer container)
		{
			AuthOptions authOptions = container.Resolve<AuthOptions>();

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