namespace OrangeJuice.Server.Configuration
{
	public sealed class WebConfigurationProvider : IConfigurationProvider
	{
		public string GetValue(string key)
		{
			return System.Web.Configuration.WebConfigurationManager.AppSettings[key];
		}
	}
}