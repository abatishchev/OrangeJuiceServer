using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	public sealed class UserRegistrationValidator : AbstractValidator<UserRegistration>
	{
		public UserRegistrationValidator()
		{
			RuleFor(x => x.Email).NotNull()
			                     .Length(6, 254)
			                     .EmailAddress();
		}
	}
}