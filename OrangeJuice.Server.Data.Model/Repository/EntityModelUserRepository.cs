using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Model.Repository
{
	public sealed class EntityModelUserRepository : IUserRepository
	{
		#region Fields
		// ReSharper disable once InconsistentNaming
		private readonly Func<IModelContainer> CreateContainer;
		#endregion

		#region Ctor
		public EntityModelUserRepository()
			: this(() => new ModelContainer())
		{
		}

		internal EntityModelUserRepository(Func<IModelContainer> createContainer)
		{
			CreateContainer = createContainer;
		}
		#endregion

		#region IUserRepository members
		public async Task<IUser> Register(string email)
		{
			using (IModelContainer db = CreateContainer())
			{
				User user = new User
				{
					Email = email
				};

				user = db.Users.Add(user);

				await db.SaveChangesAsync();
				return user;
			}
		}

		public async Task<IUser> Search(Guid userGuid)
		{
			using (IModelContainer db = CreateContainer())
			{
				return await db.Users
				               .Include(u => u.Ratings)
				               .SingleOrDefaultAsync(u => u.UserGuid == userGuid);
			}
		}
		#endregion
	}
}