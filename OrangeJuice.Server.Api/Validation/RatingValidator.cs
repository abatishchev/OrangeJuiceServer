using FluentValidation;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Validation
{
	// TODO: rating is not (can't be) decorated by this
	public sealed class RatingValidator : AbstractValidator<Rating>
	{
		public RatingValidator()
		{
			RuleFor(x => x.RatingId).NotEmpty();

			RuleFor(x => x.RatingId.UserId).NotEmpty();

			RuleFor(x => x.RatingId.ProductId).NotEmpty();

			// from 1 to 5
			RuleFor(x => x.Value).GreaterThanOrEqualTo((byte)1)
								 .LessThanOrEqualTo((byte)5);
		}
	}
}