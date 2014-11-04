using System;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Transactions;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
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
		public async Task<User> Register(string email, string name)
		{
			using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				if (await _db.Users.AnyAsync(u => u.Email == email))
					throw new DataException("User already exists");

				User user = _db.Users.Add(
					new User
					{
						Email = email,
						Name = name
					});

				await _db.SaveChangesAsync();
				scope.Complete();

				return user;
			}
		}

		public Task<User> Search(Guid userId)
		{
			return _db.Users.FindAsync(userId);
		}
		#endregion
	}
}