using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

using OrangeJuice.Server.Configuration;
using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public class ModelContext : DbContext, IModelContext
	{
		public ModelContext(IConnectionStringProvider connectionStringProvider)
			: base(connectionStringProvider.GetDefaultConnectionString())
		{
		}

		public virtual DbSet<Product> Products { get; set; }

		public virtual DbSet<Rating> Ratings { get; set; }

		public virtual DbSet<User> Users { get; set; }

		public virtual DbSet<Request> Requests { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			// dbo
			modelBuilder.Entity<Product>().HasKey(x => x.ProductId);
			modelBuilder.Entity<Product>().Property(x => x.ProductId)
						.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			modelBuilder.Entity<Rating>().HasKey(x => new { x.ProductId, x.UserId });

			modelBuilder.Entity<User>().Property(x => x.UserId)
						.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<User>().HasKey(x => x.UserId);

			// dm
			modelBuilder.Entity<Request>().HasKey(x => x.RequestId);
			modelBuilder.Entity<Request>().Property(x => x.RequestId)
						.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
		}
	}
}