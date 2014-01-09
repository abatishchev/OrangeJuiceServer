using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Unit
{
	public interface IUserUnit
	{
		Task<int> Add(User user);

		Task<User> GetUser(Guid userGuid);
	}
}