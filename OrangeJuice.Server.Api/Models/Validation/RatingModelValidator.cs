using FluentValidation;

namespace OrangeJuice.Server.Api.Models.Validation
{
	public sealed class RatingModelValidator : AbstractValidator<RatingModel>
	{
		public RatingModelValidator()
		{
			RuleFor(x => x.UserId).NotEmpty();

			RuleFor(x => x.ProductId).NotEmpty();

			// from 1 to 5
			RuleFor(x => x.Value).GreaterThanOrEqualTo((byte)1)
								 .LessThanOrEqualTo((byte)5);
		}
	}
}