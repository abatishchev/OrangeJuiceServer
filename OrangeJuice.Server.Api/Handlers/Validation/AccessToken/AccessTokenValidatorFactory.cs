using System.Net.Http;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Handlers.Validation
{
	public sealed class AccessTokenValidatorFactory : Factory.IFactory<IValidator<HttpRequestMessage, string>>
	{
		private readonly IEnvironmentProvider _environmentProvider;

		public AccessTokenValidatorFactory(IEnvironmentProvider environmentProvider)
		{
			_environmentProvider = environmentProvider;
		}

		public IValidator<HttpRequestMessage, string> Create()
		{
			string environment = _environmentProvider.GetCurrentEnvironment();
			switch (environment)
			{
				case EnvironmentName.Local:
					return new EmptyValidator<HttpRequestMessage>();
				default:
					return new HeaderAccesstTokenValidator();
			}
		}
	}
}