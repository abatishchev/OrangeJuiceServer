using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Unit;

namespace OrangeJuice.Server.Data.Repository
{
	public sealed class EntityUserRepository : IUserRepository
	{
		#region Fields
		private readonly IUserUnit _db;
		#endregion

		#region Ctor
		public EntityUserRepository(IUserUnit db)
		{
			_db = db;
		}
		#endregion

		#region IUserRepository members
		public async Task<IUser> Register(string email)
		{
			User user = new User
			{
				Email = email
			};

			await _db.Add(user);

			return user;
		}

		public async Task<IUser> Search(Guid userGuid)
		{
			return await _db.GetUser(userGuid);
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