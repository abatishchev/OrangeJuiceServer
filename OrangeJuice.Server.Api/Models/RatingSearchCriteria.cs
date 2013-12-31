using System;

namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.RatingSearchCriteriaValidator))]
	public class RatingSearchCriteria
	{
		public Guid UserGuid { get; set; }

		public string Productid { get; set; }
	}
}