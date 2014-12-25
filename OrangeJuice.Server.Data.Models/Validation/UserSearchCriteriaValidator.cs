using FluentValidation;

namespace OrangeJuice.Server.Data.Models.Validation
{
	public class UserSearchCriteriaValidator : AbstractValidator<UserSearchCriteria>
	{
		public UserSearchCriteriaValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();
		}
	}
}