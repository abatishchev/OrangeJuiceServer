using System;

namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.ProductSearchCriteriaValidator))]
	public class ProductSearchCriteria
	{
		public Guid ProductId { get; set; }
	}
}