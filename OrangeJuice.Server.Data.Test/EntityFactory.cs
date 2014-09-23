using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Data.Test
{
	public static class EntityFactory
	{
		public static T Get<T>() where T : class
		{
			try
			{
				using (var container = new ModelContainer())
				{
					var entity = container.Set<T>().FirstOrDefault();
					if (entity == null)
					{
						Assert.Inconclusive("Database contains no entities of given type");
					}
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
