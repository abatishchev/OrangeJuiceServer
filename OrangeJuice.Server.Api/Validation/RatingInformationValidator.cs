using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	public sealed class RatingInformationValidator : AbstractValidator<RatingInformation>
	{
		public RatingInformationValidator()
		{
			// from 1 to 5
			RuleFor(x => x.Value).GreaterThanOrEqualTo(1)
									   .LessThanOrEqualTo(5);
		}
	}
}