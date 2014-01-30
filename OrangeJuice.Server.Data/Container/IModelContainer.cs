using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Container
{
	public interface IModelContainer : IDisposable
	{
		#region Properties
		DbSet<Product> Products { get; }

		DbSet<Rating> Ratings { get; }

		DbSet<User> Users { get; }
		#endregion

		#region Methods
		DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

		int SaveChanges();

		Task<int> SaveChangesAsync();
		#endregion
	}
}