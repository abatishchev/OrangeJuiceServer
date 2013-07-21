using System;

namespace OrangeJuice.Server.Data
{
	public interface IUser
	{
		Guid UserGuid { get; }

		string Email { get; }
	}
}