using FluentValidation;

namespace OrangeJuice.Server.Api.Models.Validation
{
	public sealed class UserModelValidator : AbstractValidator<UserModel>
	{
		public UserModelValidator()
		{
			RuleFor(x => x.Email).NotNull()
								 .Length(6, 254)
								 .EmailAddress();

			RuleFor(x => x.Name).NotNull()
								.Length(3, 254);
		}
	}
}