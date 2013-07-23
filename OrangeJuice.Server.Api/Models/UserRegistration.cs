using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
	public class UserRegistration
	{
		[Required]
        [StringLength(36)]
		public string AppKey { get; set; }

		[Required]
		[EmailAddress]
		[StringLength(254)]
		public string Email { get; set; }
	}
}