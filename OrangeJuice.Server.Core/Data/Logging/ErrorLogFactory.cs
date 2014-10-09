using Elmah;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Data.Logging
{
	public sealed class ErrorLogFactory : Factory.IFactory<ErrorLog>
	{
		private readonly IEnvironmentProvider _environmentProvider;
		private readonly IConnectionStringProvider _connectionStringProvider;

		public ErrorLogFactory(IEnvironmentProvider environmentProvider, IConnectionStringProvider connectionStringProvider)
		{
			_environmentProvider = environmentProvider;
			_connectionStringProvider = connectionStringProvider;
		}

		public ErrorLog Create()
		{
			switch (_environmentProvider.GetCurrentEnvironment())
			{
				case Environment.Production:
					return new Elmah.Contrib.EntityFramework.EntityErrorLog(_connectionStringProvider.GetDefaultConnectionString());
				default:
					return new TraceErrorLog();
			}
		}
	}
}