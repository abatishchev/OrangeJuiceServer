using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Unit;

namespace OrangeJuice.Server.Data.Repository
{
	public sealed class EntityUserRepository : IUserRepository
	{
		#region Fields
		private readonly IUserUnit _userUnit;
		#endregion

		#region Ctor
		public EntityUserRepository(IUserUnit userUnit)
		{
			_userUnit = userUnit;
		}
		#endregion

		#region IUserRepository members
		public async Task<IUser> Register(string name, string email)
		{
			User user = new User
			{
				Name = name,
				Email = email
			};

			return await _userUnit.Add(user);
		}

		public async Task<IUser> Search(Guid userGuid)
		{
			return await _userUnit.Get(userGuid);
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_userUnit.Dispose();
		}
		#endregion
	}
}