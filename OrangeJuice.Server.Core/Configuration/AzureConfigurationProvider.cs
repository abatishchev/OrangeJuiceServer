namespace OrangeJuice.Server.Configuration
{
	public sealed class AzureConfigurationProvider : IConfigurationProvider
	{
		public string GetValue(string key)
		{
			return Microsoft.Azure.CloudConfigurationManager.GetSetting(key);
		}
	}
}