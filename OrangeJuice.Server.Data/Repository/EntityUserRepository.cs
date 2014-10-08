using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Context;

namespace OrangeJuice.Server.Data.Repository
{
	public sealed class EntityUserRepository : IUserRepository
	{
		#region Fields
		private readonly IModelContext _db;
		#endregion

		#region Ctor
		public EntityUserRepository(IModelContext db)
		{
			_db = db;
		}
		#endregion

		#region IUserRepository members
		public async Task<IUser> Register(string email, string name)
		{
			User user = _db.Users.Add(
				new User
				{
					Email = email,
					Name = name
				});

			await _db.SaveChangesAsync();
			return user;
		}

		public async Task<IUser> Search(Guid userId)
		{
			return await _db.Users.FindAsync(userId);
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_db.Dispose();
		}
		#endregion
	}
}