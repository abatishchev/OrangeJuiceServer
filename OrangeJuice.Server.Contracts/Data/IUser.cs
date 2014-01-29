using System;

namespace OrangeJuice.Server.Data
{
	public interface IUser
	{
		Guid UserId { get; }

		string Email { get; set; }

		string Name { get; set; }
	}
}