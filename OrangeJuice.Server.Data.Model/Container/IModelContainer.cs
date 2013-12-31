using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace OrangeJuice.Server.Data.Model
{
	internal interface IModelContainer : IDisposable
	{
		#region Properties
		DbSet<User> Users { get; set; }

		DbSet<Rating> Ratings { get; set; }
		#endregion

		#region Methods
		DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

		Task<int> SaveChangesAsync();
		#endregion
	}
}