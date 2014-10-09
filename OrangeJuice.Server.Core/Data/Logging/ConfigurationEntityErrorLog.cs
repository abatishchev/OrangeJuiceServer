using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Data.Logging
{
	public sealed class ConfigurationEntityErrorLog : Elmah.Contrib.EntityFramework.EntityErrorLog
	{
		public ConfigurationEntityErrorLog(IConfigurationProvider configurationProvider)
			: base(configurationProvider.GetValue("sql:ConnectionString"))
		{
		}
	}
}