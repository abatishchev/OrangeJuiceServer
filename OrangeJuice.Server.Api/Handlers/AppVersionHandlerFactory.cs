using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class AppVersionHandlerFactory : IFactory<AppVersionHandler>
	{
		private readonly IEnvironmentProvider _environmentProvider;

		public AppVersionHandlerFactory(IEnvironmentProvider environmentProvider)
		{
			_environmentProvider = environmentProvider;
		}

		public AppVersionHandler Create()
		{
			string environment = _environmentProvider.GetCurrentEnvironment();
			switch (environment)
			{
				case Environment.Local:
					return new EmptyAppVersionHandler();
				case Environment.Production:
					return new HeaderAppVersionHandler(AppVersion.Version0);
				default:
					return new QueryAppVersionHandler(AppVersion.Version0);
			}
		}
	}
}