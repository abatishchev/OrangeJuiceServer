using FluentValidation;

namespace OrangeJuice.Server.Api.Models.Validation
{
	public class RatingsSearchCriteriaValidator : AbstractValidator<RatingsSearchCriteria>
	{
		public RatingsSearchCriteriaValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty();
		}
	}
}