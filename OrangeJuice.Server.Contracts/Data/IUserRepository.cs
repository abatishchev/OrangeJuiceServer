using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public interface IUserRepository : IDisposable
	{
		Task<User> Register(string email, string name);

        Task<User> Search(Guid userId);
	}
}