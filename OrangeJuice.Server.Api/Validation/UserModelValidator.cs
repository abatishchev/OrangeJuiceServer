using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
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