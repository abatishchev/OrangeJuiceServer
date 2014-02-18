using System.Configuration;

namespace OrangeJuice.Server.Configuration
{
	public class AppSettingsConfigurationProvider : IConfigurationProvider
	{
		#region IConfigurationProvider members
		public virtual string GetValue(string key)
		{
			return ConfigurationManager.AppSettings[key];
		}
		#endregion
	}
}