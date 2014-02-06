﻿using FluentValidation;

namespace OrangeJuice.Server.Api.Validation
{
	public class ProductSearchCriteriaValidator : AbstractValidator<Models.ProductSearchCriteria>
	{
		public ProductSearchCriteriaValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty();
		}
	}
}