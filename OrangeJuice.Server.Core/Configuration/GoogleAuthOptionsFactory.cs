namespace OrangeJuice.Server.Configuration
{
	public sealed class GoogleAuthOptionsFactory : Factory.IFactory<GoogleAuthOptions>
	{
		private readonly IConfigurationProvider _configurationProvider;

		public GoogleAuthOptionsFactory(IConfigurationProvider configurationProvider)
		{
			_configurationProvider = configurationProvider;
		}

		public GoogleAuthOptions Create()
		{
			return new GoogleAuthOptions
			{
				Audience = _configurationProvider.GetValue("google:Audience"),
				CertificateKey = _configurationProvider.GetValue("google:CertificateKey"),
				CertificateSecret = _configurationProvider.GetValue("google:CertificateSecret"),
				ClientId = _configurationProvider.GetValue("google:ClientId"),
				Issuer = _configurationProvider.GetValue("google:Issuer")
			};
		}
	}
}