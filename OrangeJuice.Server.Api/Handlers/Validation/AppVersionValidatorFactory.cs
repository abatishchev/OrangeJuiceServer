using System.Net.Http;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Handlers.Validation
{
	public sealed class AppVersionValidatorFactory : IFactory<IValidator<HttpRequestMessage>>
	{
		private readonly IEnvironmentProvider _environmentProvider;

		public AppVersionValidatorFactory(IEnvironmentProvider environmentProvider)
		{
			_environmentProvider = environmentProvider;
		}

		public IValidator<HttpRequestMessage> Create()
		{
			string environment = _environmentProvider.GetCurrentEnvironment();
			switch (environment)
			{
				case Environment.Local:
					return new EmptyAppVersionValidator();
				default:
					return new HeaderAppVersionValidator(AppVersion.Version0);
			}
		}
	}
}