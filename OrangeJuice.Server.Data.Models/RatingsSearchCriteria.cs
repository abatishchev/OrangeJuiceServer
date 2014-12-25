using System;

using FluentValidation.Attributes;

namespace OrangeJuice.Server.Data.Models
{
	[Validator(typeof(Validation.RatingsSearchCriteriaValidator))]
	public class RatingsSearchCriteria
	{
		public Guid ProductId { get; set; }
	}
}