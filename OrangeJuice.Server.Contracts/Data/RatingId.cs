using System;

namespace OrangeJuice.Server.Data
{
	public struct RatingId
	{
		public Guid UserId { get; set; }

		public Guid ProductId { get; set; }
	}
}