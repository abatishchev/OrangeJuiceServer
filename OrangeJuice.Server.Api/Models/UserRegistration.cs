using System.ComponentModel.DataAnnotations;

namespace OrangeJuice.Server.Api.Models
{
	/// <seealso cref="OrangeJuice.Server.Api.Validation.UserRegistrationValidator" />
	public class UserRegistration
	{
		public string Email { get; set; }
	}
}