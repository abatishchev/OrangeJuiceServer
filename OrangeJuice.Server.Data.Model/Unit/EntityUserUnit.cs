using System;
using System.Data.Entity;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Container;

namespace OrangeJuice.Server.Data.Unit
{
	public sealed class EntityUserUnit : IUserUnit
	{
		#region Fields
		private readonly IFactory<IModelContainer> _containerFactory;
		#endregion

		#region Ctor
		public EntityUserUnit(IFactory<IModelContainer> containerFactory)
		{
			_containerFactory = containerFactory;
		}
		#endregion

		#region IUserUnit members
		public Task<int> Add(User user)
		{
			using (IModelContainer db = _containerFactory.Create())
			{
				db.Users.Add(user);

				return db.SaveChangesAsync();
			}
		}

		public Task<User> GetUser(Guid userGuid)
		{
			using (IModelContainer db = _containerFactory.Create())
			{
				return db.Users.SingleOrDefaultAsync(u => u.UserGuid == userGuid);
			}
		}
		#endregion
	}
}