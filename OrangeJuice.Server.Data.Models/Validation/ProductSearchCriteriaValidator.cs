using FluentValidation;

namespace OrangeJuice.Server.Data.Models.Validation
{
	public class ProductSearchCriteriaValidator : AbstractValidator<ProductSearchCriteria>
	{
		public ProductSearchCriteriaValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty();
		}
	}
}