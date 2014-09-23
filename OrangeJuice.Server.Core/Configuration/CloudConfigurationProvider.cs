namespace OrangeJuice.Server.Configuration
{
	public sealed class CloudConfigurationProvider : IConfigurationProvider
	{
		public string GetValue(string key)
		{
			return Microsoft.WindowsAzure.CloudConfigurationManager.GetSetting(key);
		}
	}
}