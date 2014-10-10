﻿using System.Data;
using System.Linq;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Data.Test
{
	public static class EntityFactory
	{
		public static T Get<T>() where T : class
		{
			using (var container = new ModelContext(new ConfigurationConnectionStringProvider(new AppSettingsConfigurationProvider())))
			{
				var entity = container.Set<T>().FirstOrDefault();
				if (entity == null)
					throw new DataException("Database contains no entities of given type");
				return entity;
			}
		}
	}
}