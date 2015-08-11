using System;
using System.Net.Http;
using System.Net.Http.Headers;

using Factory;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Validation;

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