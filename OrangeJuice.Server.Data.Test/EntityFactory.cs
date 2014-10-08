﻿using System;
using System.Data;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Configuration;

namespace OrangeJuice.Server.Data.Test
{
	public static class EntityFactory
	{
		public static T Get<T>() where T : class
		{
			try
			{
				using (var container = new ModelContext(new AppSettingsConfigurationProvider()))
				{
					var entity = container.Set<T>().FirstOrDefault();
					if (entity == null)
						throw new DataException("Database contains no entities of given type");
					return entity;
				}
			}
			catch (Exception ex)
			{
				Assert.Inconclusive(ex.Message);
			}
			return null;
		}
	}
}
