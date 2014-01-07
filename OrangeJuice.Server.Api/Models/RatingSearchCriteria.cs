using System;

namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.RatingSearchCriteriaValidator))]
	public class RatingSearchCriteria
	{
		public string Productid { get; set; }

		public Guid UserGuid { get; set; }
	}
}