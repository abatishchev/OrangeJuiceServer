using System;

using FluentValidation.Attributes;
using OrangeJuice.Server.Data.Models.Validation;

namespace OrangeJuice.Server.Data.Models
{
	[Validator(typeof(ProductSearchCriteriaValidator))]
	public class ProductSearchCriteria
	{
		public Guid ProductId { get; set; }
	}
}