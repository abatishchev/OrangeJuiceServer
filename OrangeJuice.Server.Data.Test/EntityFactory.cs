using System.Linq;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Data.Test
{
	public static class EntityFactory
	{
		public static T Get<T>() where T : class
		{
			try
			{
				using (var container = new ModelContext(new ConfigurationConnectionStringProvider(new AppSettingsConfigurationProvider())))
				{
					return container.Set<T>().FirstOrDefault();
				}
			}
			catch
			{
				return null;
			}
		}
	}
}