using System;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Api.Handlers
{
	public sealed class AppKeyHandlerFactory : IFactory<AppKeyHandlerBase>
	{
		private readonly IEnvironmentProvider _environmentProvider;

		public AppKeyHandlerFactory(IEnvironmentProvider environmentProvider)
		{
			if (environmentProvider == null)
				throw new ArgumentNullException("environmentProvider");
			_environmentProvider = environmentProvider;
		}

		public AppKeyHandlerBase Create()
		{
			string environment = _environmentProvider.GetCurrentEnvironment();
			switch (environment)
			{
				case Configuration.Environment.Local:
					return new EmptyAppKeyHandler();
				case Configuration.Environment.Production:
					return new HeaderAppKeyHandler(AppKey.Version0);
				default:
					return new QueryAppKeyHandler(AppKey.Version0);
			}
		}
	}
}