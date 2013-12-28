using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class AppKeyHandlerFactory : IFactory<AppKeyHandlerBase>
	{
		private readonly IEnvironmentProvider _environmentProvider;

		public AppKeyHandlerFactory(IEnvironmentProvider environmentProvider)
		{
			_environmentProvider = environmentProvider;
		}

		public AppKeyHandlerBase Create()
		{
			string environment = _environmentProvider.GetCurrentEnvironment();
			switch (environment)
			{
				case Environment.Local:
					return new EmptyAppKeyHandler();
				case Environment.Production:
					return new HeaderAppKeyHandler(AppKey.Version0);
				default:
					return new QueryAppKeyHandler(AppKey.Version0);
			}
		}
	}
}