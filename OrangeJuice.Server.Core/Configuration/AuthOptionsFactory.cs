namespace OrangeJuice.Server.Configuration
{
	public sealed class AuthOptionsFactory : Factory.IFactory<AuthOptions>
	{
		private readonly IConfigurationProvider _configurationProvider;

		public AuthOptionsFactory(IConfigurationProvider configurationProvider)
		{
			_configurationProvider = configurationProvider;
		}

		public AuthOptions Create()
		{
			return new AuthOptions
			{
				Audience = _configurationProvider.GetValue("auth0:Audience"),
				CertificateKey = _configurationProvider.GetValue("auth0:CertificateKey"),
				Issuer = _configurationProvider.GetValue("auth0:Issuer")
			};
		}
	}
}