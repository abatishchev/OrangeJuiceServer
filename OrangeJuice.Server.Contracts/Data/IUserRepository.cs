using System;

namespace OrangeJuice.Server.Data
{
	public interface IUserRepository
	{
		IUser Find(Guid userGuid);

		IUser Register(string email);
	}
}