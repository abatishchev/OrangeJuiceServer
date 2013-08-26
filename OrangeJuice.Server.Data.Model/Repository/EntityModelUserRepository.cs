using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Model.Repository
{
	public sealed class EntityModelUserRepository : IUserRepository
	{
		public Task<IUser> Register(string email)
		{
			using (var db = new ModelContainer())
			{
				User user = User.CreateNew(email);
				if (user.UserGuid == Guid.Empty)
					throw new DataException("User guid can't be empty");

				user = db.Users.Add(user);
				int i = db.SaveChanges();
				if (i != 1)
					throw new DataException("Error saving user");

				return Task.FromResult<IUser>(user);
			}
		}

		public Task<IUser> SearchByGuid(Guid userGuid)
		{
			using (var db = new ModelContainer())
			{
				return Task.FromResult<IUser>(db.Users.SingleOrDefault(u => u.UserGuid == userGuid));
			}
		}
	}
}