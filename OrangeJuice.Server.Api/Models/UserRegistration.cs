using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
	public class UserRegistration
	{
		[Required]
		public Guid? AppKey { get; set; }

		[Required]
		[EmailAddress]
		[StringLength(254, MinimumLength = 6)]
		public string Email { get; set; }
	}
}