using System;

using OrangeJuice.Server.Api.Validation;

namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(RatingSearchCriteriaValidator))]
	public class RatingSearchCriteria
	{
		public Guid UserId { get; set; }

		public Guid ProductId { get; set; }
	}
}