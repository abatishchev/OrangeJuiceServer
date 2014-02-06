﻿using System;

using FluentValidation.Attributes;

using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Models
{
	[Validator(typeof(RatingsSearchCriteriaValidator))]
	public class RatingsSearchCriteria
	{
		public Guid ProductId { get; set; }
	}
}