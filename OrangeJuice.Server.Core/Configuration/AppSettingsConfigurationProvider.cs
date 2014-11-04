namespace OrangeJuice.Server.Configuration
{
	public sealed class AppSettingsConfigurationProvider : IConfigurationProvider
	{
		public string GetValue(string key)
		{
			return System.Configuration.ConfigurationManager.AppSettings[key];
		}
	}
}