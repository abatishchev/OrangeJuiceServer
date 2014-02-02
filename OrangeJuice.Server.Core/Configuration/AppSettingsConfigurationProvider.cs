namespace OrangeJuice.Server.Configuration
{
	public class AppSettingsConfigurationProvider : IConfigurationProvider
	{
		public virtual string GetValue(string key)
		{
			return System.Configuration.ConfigurationManager.AppSettings[key];
		}
	}
}