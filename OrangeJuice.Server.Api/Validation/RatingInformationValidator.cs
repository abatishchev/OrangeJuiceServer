using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	public sealed class RatingInformationValidator : AbstractValidator<RatingInformation>
	{
		public RatingInformationValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();

			RuleFor(x => x.ProductId).NotEmpty();

			// from 1 to 5
			RuleFor(x => x.Value).GreaterThanOrEqualTo((byte)1)
								 .LessThanOrEqualTo((byte)5);
		}
	}
}