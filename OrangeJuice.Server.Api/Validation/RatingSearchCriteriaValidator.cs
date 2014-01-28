using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	public sealed class RatingSearchCriteriaValidator : AbstractValidator<RatingSearchCriteria>
	{
		public RatingSearchCriteriaValidator()
		{
			RuleFor(x => x.RatingId).NotEmpty();
			
			// TODO: is it required?
			RuleFor(x => x.RatingId.UserId).NotEmpty();
			RuleFor(x => x.RatingId.ProductId).NotEmpty();
		}
	}
}