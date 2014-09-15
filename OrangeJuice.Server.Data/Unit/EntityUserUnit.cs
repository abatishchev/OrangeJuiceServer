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
		public Task Add(User user)
		{
			_container.Users.Add(user);

			return _container.SaveChangesAsync();
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