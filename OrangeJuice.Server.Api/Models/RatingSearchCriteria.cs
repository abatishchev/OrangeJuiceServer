using System;

using FluentValidation.Attributes;

namespace OrangeJuice.Server.Api.Models
{
	[Validator(typeof(Validation.RatingSearchCriteriaValidator))]
	public class RatingSearchCriteria
	{
		public Guid UserId { get; set; }

		public Guid ProductId { get; set; }
	}
}