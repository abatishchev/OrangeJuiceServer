using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Model.Repository
{
	public sealed class EntityModelUserRepository : IUserRepository
	{
		#region Fields
		private readonly IFactory<IModelContainer> _containerFactory;
		#endregion

		#region Ctor
		public EntityModelUserRepository(IFactory<IModelContainer> containerFactory)
		{
			_containerFactory = containerFactory;
		}
		#endregion

		#region IUserRepository members
		public async Task<IUser> Register(string email)
		{
			using (IModelContainer db = _containerFactory.Create())
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
			using (IModelContainer db = _containerFactory.Create())
			{
				return await db.Users
							   .Include(u => u.Ratings)
							   .SingleOrDefaultAsync(u => u.UserGuid == userGuid);
			}
		}
		#endregion
	}
}