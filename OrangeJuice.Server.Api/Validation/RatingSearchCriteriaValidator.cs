using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	public sealed class RatingSearchCriteriaValidator : AbstractValidator<RatingSearchCriteria>
	{
		public RatingSearchCriteriaValidator()
		{
			RuleFor(x => x.Productid).NotNull()
									 .Length(10); // ASIN

			RuleFor(x => x.UserGuid).NotEmpty();
		}
	}
}