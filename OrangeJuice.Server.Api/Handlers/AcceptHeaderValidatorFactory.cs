using System.Net.Http;

using Ab.Configuration;
using Ab.Factory;
using Ab.Validation;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class AcceptHeaderValidatorFactory : IFactory<IValidator<HttpRequestMessage>>
	{
		private readonly IEnvironmentProvider _environmentProvider;

		public AcceptHeaderValidatorFactory(IEnvironmentProvider environmentProvider)
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
					return new AcceptHeaderValidator(AppVersion.Version0);
			}
		}
	}
}