using Microsoft.WindowsAzure;

namespace OrangeJuice.Server.Configuration
{
	public sealed class ClouConfigurationProvider : AppSettingsConfigurationProvider
	{
		#region IConfigurationProvider members
		public override string GetValue(string key)
		{
			return CloudConfigurationManager.GetSetting(key);
		}
		#endregion

	}
}