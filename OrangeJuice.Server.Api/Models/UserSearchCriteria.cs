using System;
using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
	public class UserSearchCriteria
	{
		[Required]
		public Guid? UserGuid { get; set; }
	}
}