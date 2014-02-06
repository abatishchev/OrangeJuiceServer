using FluentValidation;

namespace OrangeJuice.Server.Api.Validation
{
	public class RatingSearchCriteriaValidator : AbstractValidator<Models.RatingSearchCriteria>
	{
		public RatingSearchCriteriaValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();

			RuleFor(x => x.ProductId).NotEmpty();
		}
	}
}