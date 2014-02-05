using System;

namespace OrangeJuice.Server.Data
{
	public interface IRating
	{
		Guid UserId { get; }

		Guid ProductId { get; }

		byte Value { get; set; }

		string Comment { get; set; }
	}
}