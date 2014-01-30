using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Container;

namespace OrangeJuice.Server.Data.Unit
{
	public sealed class EntityUserUnit : IUserUnit
	{
		#region Fields
		private readonly IModelContainer _container;
		#endregion

		#region Ctor
		public EntityUserUnit(IModelContainer container)
		{
			_container = container;
		}
		#endregion

		#region IUserUnit members
		public async Task<User> Add(User user)
		{
			user = _container.Users.Add(user);

			await _container.SaveChangesAsync();

			return user;
		}

		public Task<User> Get(Guid userId)
		{
			return _container.Users.FindAsync(userId);
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_container.Dispose();
		}
		#endregion
	}
}