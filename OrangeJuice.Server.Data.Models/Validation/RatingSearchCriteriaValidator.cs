using FluentValidation;

namespace OrangeJuice.Server.Data.Models.Validation
{
	public class RatingSearchCriteriaValidator : AbstractValidator<RatingSearchCriteria>
	{
		public RatingSearchCriteriaValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();

			RuleFor(x => x.ProductId).NotEmpty();
		}
	}
}