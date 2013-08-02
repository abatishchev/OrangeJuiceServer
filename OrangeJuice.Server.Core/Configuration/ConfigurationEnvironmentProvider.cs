using System;

namespace OrangeJuice.Server.Configuration
{
	public sealed class ConfigurationEnvironmentProvider : IEnvironmentProvider
	{
		internal const string KeyName = "Environment";

		private readonly IConfigurationProvider _configurationProvider;

		public ConfigurationEnvironmentProvider(IConfigurationProvider configurationProvider)
		{
			if (configurationProvider == null)
				throw new ArgumentNullException("configurationProvider");

			_configurationProvider = configurationProvider;
		}

		public string GetCurrentEnvironment()
		{
			string environment = _configurationProvider.GetValue(KeyName);
			if (String.IsNullOrEmpty(environment))
				throw new InvalidOperationException("Current environment is null or empty");
			return environment;
		}
	}
}