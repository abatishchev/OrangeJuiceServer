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
		public async Task<IUser> Register(string email)
		{
			User user = new User
			{
				Email = email
			};

			await _userUnit.Add(user);

			return user;
		}

		public async Task<IUser> Search(Guid userGuid)
		{
			return await _userUnit.GetUser(userGuid);
		}
		#endregion
	}
}