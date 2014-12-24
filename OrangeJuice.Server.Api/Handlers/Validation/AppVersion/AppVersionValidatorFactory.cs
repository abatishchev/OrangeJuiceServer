using System.Net.Http;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Validation;

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
			switch (_environmentProvider.GetCurrentEnvironment())
			{
				case EnvironmentName.Local:
					return new EmptyValidator<HttpRequestMessage>();
				default:
					return new HeaderAppVersionValidator(AppVersion.Version0);
			}
		}
	}
}