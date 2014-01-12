using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Unit
{
	public interface IUserUnit : IDisposable
	{
		Task<int> Add(User user);

		Task<User> Get(Guid userGuid);
	}
}