using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	public sealed class TitleSearchCriteriaValidator : AbstractValidator<TitleSearchCriteria>
	{
		public TitleSearchCriteriaValidator()
		{
			RuleFor(x => x.Title).NotNull();
			RuleFor(x => x.Title).Length(3, 56);
		}
	}
}