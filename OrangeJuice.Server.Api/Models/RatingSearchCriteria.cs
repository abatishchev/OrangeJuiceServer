using System;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.RatingSearchCriteriaValidator))]
	public class RatingSearchCriteria
	{
		public RatingId RatingId { get; set; }
	}
}