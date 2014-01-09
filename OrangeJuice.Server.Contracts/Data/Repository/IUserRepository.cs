﻿using System;
using System.Threading.Tasks;

namespace OrangeJuice.Server.Data.Repository
{
	public interface IUserRepository : IDisposable
	{
		Task<IUser> Register(string email);

		Task<IUser> Search(Guid userGuid);
	}
}