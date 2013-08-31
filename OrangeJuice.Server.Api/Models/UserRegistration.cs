namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.UserRegistrationValidator))]
	public class UserRegistration
	{
		public string Email { get; set; }
	}
}