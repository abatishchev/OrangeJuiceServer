﻿using System;

using FluentValidation.Attributes;

namespace OrangeJuice.Server.Api.Models
{
	[Validator(typeof(Validation.ProductSearchCriteriaValidator))]
	public class ProductSearchCriteria
	{
		public Guid ProductId { get; set; }
	}
}