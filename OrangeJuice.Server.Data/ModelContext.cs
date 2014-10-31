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

		protected override void OnModelCreating(DbModelBuilder builder)
		{
			// dbo
			builder.Entity<Product>().HasKey(x => x.ProductId);
			builder.Entity<Product>().Property(x => x.ProductId)
						.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			builder.Entity<Rating>().HasKey(x => new { x.ProductId, x.UserId });

			builder.Entity<User>().Property(x => x.UserId)
						.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			builder.Entity<User>().HasKey(x => x.UserId);

			// dm
			builder.Entity<Request>().ToTable("Requests", "dm").HasKey(x => x.RequestId);
			builder.Entity<Request>().Property(x => x.RequestId)
						.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			base.OnModelCreating(builder);
		}
	}
}