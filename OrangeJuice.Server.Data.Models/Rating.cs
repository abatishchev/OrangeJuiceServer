using System;

namespace OrangeJuice.Server.Data.Models
{
	public class Rating
	{
		public Guid UserId { get; set; }

		public Guid ProductId { get; set; }

		public byte Value { get; set; }

		public string Comment { get; set; }

		public virtual Product Product { get; set; }

		public virtual User User { get; set; }
	}
}