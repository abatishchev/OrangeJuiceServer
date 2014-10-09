namespace OrangeJuice.Server.Configuration
{
	public class ConfigurationConnectionStringProvider : IConnectionStringProvider
	{
		private readonly IConfigurationProvider _configurationProvider;

		public ConfigurationConnectionStringProvider(IConfigurationProvider configurationProvider)
		{
			_configurationProvider = configurationProvider;
		}

		public string GetDefaultConnectionString()
		{
			return _configurationProvider.GetValue("sql:ConnectionString");
		}
	}
}