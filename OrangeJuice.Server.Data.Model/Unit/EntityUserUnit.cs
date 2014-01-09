using System;
using System.Data.Entity;
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
		public Task<int> Add(User user)
		{
			_container.Users.Add(user);

			return _container.SaveChangesAsync();
		}

		public Task<User> GetUser(Guid userGuid)
		{
			return _container.Users.SingleOrDefaultAsync(u => u.UserGuid == userGuid);
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