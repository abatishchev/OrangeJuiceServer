using System.Net.Http;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Handlers.Validation
{
	public sealed class AppVersionValidatorFactory : Factory.IFactory<IValidator<HttpRequestMessage>>
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
				case EnvironmentName.Local:
					return new EmptyValidator<HttpRequestMessage>();
				default:
					return new HeaderAppVersionValidator(AppVersion.Version0);
			}
		}
	}
}