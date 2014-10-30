using System.Web.Configuration;

namespace OrangeJuice.Server.Configuration
{
	public sealed class WebConfigurationProvider : IConfigurationProvider
	{
		public string GetValue(string key)
		{
			return WebConfigurationManager.AppSettings[key];
		}
	}
}