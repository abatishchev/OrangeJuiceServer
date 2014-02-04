using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	public class ProductSearchCriteriaValidator : AbstractValidator<ProductSearchCriteria>
	{
		public ProductSearchCriteriaValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty();
		}
	}
}