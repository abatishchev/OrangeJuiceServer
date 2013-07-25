using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
	public class UserInformation
	{
		[Required]
		public Guid? UserKey { get; set; }
	}
}