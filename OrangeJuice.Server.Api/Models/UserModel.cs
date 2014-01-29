namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.UserModelValidator))]
	public class UserModel
	{
		public string Email { get; set; }

		public string Name { get; set; }
	}
}