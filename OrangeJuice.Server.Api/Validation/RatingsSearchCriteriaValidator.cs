using FluentValidation;

namespace OrangeJuice.Server.Api.Validation
{
	public class RatingsSearchCriteriaValidator : AbstractValidator<Models.RatingsSearchCriteria>
	{
		public RatingsSearchCriteriaValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty();
		}
	}
}