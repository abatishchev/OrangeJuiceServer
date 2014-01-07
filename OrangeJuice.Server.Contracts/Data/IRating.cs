using System;

namespace OrangeJuice.Server.Data
{
	public interface IRating
	{
		string ProductId { get; }

		Guid UserGuid { get; }

		byte Value { get; }
	}
}