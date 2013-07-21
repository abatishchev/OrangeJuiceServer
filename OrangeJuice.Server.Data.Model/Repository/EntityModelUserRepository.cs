using System;
using System.Data;
using System.Linq;

namespace OrangeJuice.Server.Data.Model.Repository
{
	public sealed class EntityModelUserRepository : IUserRepository
	{
		public IUser Find(Guid userGuid)
		{
			using (var db = new ModelContainer())
			{
				return db.Users.SingleOrDefault(u => u.UserGuid == userGuid);
			}
		}

		public IUser Register(string email)
		{
			using (var db = new ModelContainer())
			{
				User user = User.CreateNew();

				user = db.Users.Add(user);

				if (user.UserGuid == Guid.Empty)
					throw new DataException("User guid can't be empty");

				return user;
			}
		}
	}
}