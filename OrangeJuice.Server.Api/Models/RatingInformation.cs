using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.RatingInformationValidator))]
	// TODO: remove in favor of Data.Rating
	public class RatingInformation
	{
		public RatingId RatingId { get; set; }

		public int Value { get; set; }

		public string Comment { get; set; }
	}
}