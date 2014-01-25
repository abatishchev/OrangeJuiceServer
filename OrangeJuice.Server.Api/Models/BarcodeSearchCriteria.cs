using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Models
{
	[FluentValidation.Attributes.Validator(typeof(Validation.BarcodeSearchCriteriaValidator))]
	public class BarcodeSearchCriteria
	{
		public string Barcode { get; set; }

		public BarcodeType BarcodeType { get; set; }
	}
}