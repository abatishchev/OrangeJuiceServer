using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public interface IModelContext : IDisposable
	{
		#region Schema dbo
		DbSet<Product> Products { get; }

		DbSet<Rating> Ratings { get; }

		DbSet<User> Users { get; }
		#endregion

		#region Schema dm
		DbSet<Request> Requests { get; }
		#endregion

		#region Methods
		DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

		Task<int> SaveChangesAsync();
		#endregion
	}
}