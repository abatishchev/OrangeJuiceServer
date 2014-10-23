using System;
using System.Collections.Generic;

namespace OrangeJuice.Server.Data.Models
{
	public class User
	{
		public User()
		{
			this.Ratings = new HashSet<Rating>();
		}

		public Guid UserId { get; set; }

		public string Email { get; set; }

		public string Name { get; set; }

		public virtual ICollection<Rating> Ratings { get; set; }
	}
}