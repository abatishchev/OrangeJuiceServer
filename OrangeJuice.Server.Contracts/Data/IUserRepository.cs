using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data
{
	public interface IUserRepository
	{
		Task<IUser> Find(Guid userGuid);

		Task<IUser> Register(string email);
	}
}