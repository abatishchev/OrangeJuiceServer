using System;

using FluentValidation.Attributes;

using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Models
{
	[Validator(typeof(RatingSearchCriteriaValidator))]
	public class RatingSearchCriteria
	{
		public Guid UserId { get; set; }

		public Guid ProductId { get; set; }
	}
}