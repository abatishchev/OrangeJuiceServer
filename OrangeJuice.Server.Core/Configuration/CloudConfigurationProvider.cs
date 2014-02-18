namespace OrangeJuice.Server.Configuration
{
	public sealed class CloudConfigurationProvider : AppSettingsConfigurationProvider
	{
		#region IConfigurationProvider members
		public override string GetValue(string key)
		{
			return Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting(key);
		}
		#endregion
	}
}