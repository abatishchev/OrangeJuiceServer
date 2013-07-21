using System;

namespace OrangeJuice.Server.Data
{
	public interface IUserRepository
	{
		IUser Get(Guid userGuid);

		Guid Register(string email);
	}
}