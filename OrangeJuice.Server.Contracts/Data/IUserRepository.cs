using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public interface IUserRepository
	{
		Task<User> Register(string email, string name);

		Task<User> Search(Guid userId);

		Task Update(string email, string name);
	}
}