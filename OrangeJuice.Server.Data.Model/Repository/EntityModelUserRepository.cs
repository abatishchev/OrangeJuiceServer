using System;
using System.Data;

namespace OrangeJuice.Server.Data.Model.Repository
{
	public sealed class EntityModelUserRepository : IUserRepository
	{
		public Guid Register(string email)
		{
			using (var db = new ModelContainer())
			{
				User user = User.CreateNew();

				user = db.Users.Add(user);

				if (user.UserGuid == Guid.Empty)
					throw new DataException("User guid can't be empty");

				return user.UserGuid;
			}
		}
	}
}