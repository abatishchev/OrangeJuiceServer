using System;

namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.RatingInformationValidator))]
	public class RatingInformation
	{
		public Guid UserGuid { get; set; }

		public string Productid { get; set; }

		public int Value { get; set; }
	}
}