using FluentValidation.Attributes;

namespace OrangeJuice.Server.Api.Models
{
	[Validator(typeof(Validation.BarcodeSearchCriteriaValidator))]
	public class BarcodeSearchCriteria
	{
		public string Barcode { get; set; }

		public Data.BarcodeType BarcodeType { get; set; }
	}
}