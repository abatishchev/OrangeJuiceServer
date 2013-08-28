using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	public sealed class FoodSearchCriteriaValidator : AbstractValidator<FoodSearchCriteria>
	{
		public FoodSearchCriteriaValidator()
		{
			RuleFor(x => x.Title).NotNull();
			RuleFor(x => x.Title).Length(3, 56);
		}
	}
}