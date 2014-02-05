using FluentValidation;

namespace OrangeJuice.Server.Api.Validation
{
	public class RatingSearchCriteriaValidator : AbstractValidator<Models.RatingSearchCriteria>
	{
		public RatingSearchCriteriaValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty();

			// UserId may be missing thus empty
		}
	}
}