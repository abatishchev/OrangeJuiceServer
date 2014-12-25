using FluentValidation;

namespace OrangeJuice.Server.Data.Models.Validation
{
	public class RatingsSearchCriteriaValidator : AbstractValidator<RatingsSearchCriteria>
	{
		public RatingsSearchCriteriaValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty();
		}
	}
}