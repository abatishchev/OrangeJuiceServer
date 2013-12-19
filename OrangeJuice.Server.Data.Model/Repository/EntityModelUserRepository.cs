using System;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Model.Repository
{
	public sealed class EntityModelUserRepository : IUserRepository
	{
		public async Task<IUser> Register(string email)
		{
			using (var db = new ModelContainer())
			{
				User user = new User
				{
					Email = email
				};

				user = db.Users.Add(user);

				int r = await db.SaveChangesAsync();
				if (r != 1)
					throw new DataException("Error saving user");

				return user;
			}
		}

		public async Task<IUser> SearchByGuid(Guid userGuid)
		{
			using (var db = new ModelContainer())
			{
				return await db.Users.SingleOrDefaultAsync(u => u.UserGuid == userGuid);
			}
		}
	}
}