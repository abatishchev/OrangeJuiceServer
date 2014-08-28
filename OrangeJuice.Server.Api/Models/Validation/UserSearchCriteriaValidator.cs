using FluentValidation;

namespace OrangeJuice.Server.Api.Models.Validation
{
	public class UserSearchCriteriaValidator : AbstractValidator<UserSearchCriteria>
	{
		public UserSearchCriteriaValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();
		}
	}
}