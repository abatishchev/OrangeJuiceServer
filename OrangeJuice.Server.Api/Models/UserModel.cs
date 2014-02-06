using FluentValidation.Attributes;

namespace OrangeJuice.Server.Api.Models
{
	[Validator(typeof(Validation.UserModelValidator))]
	public class UserModel
	{
		public string Email { get; set; }

		public string Name { get; set; }
	}
}