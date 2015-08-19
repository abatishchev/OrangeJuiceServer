using Ab.Amazon.Data;
using FluentValidation.Attributes;

namespace OrangeJuice.Server.Data.Models
{
	[Validator(typeof(Validation.BarcodeSearchCriteriaValidator))]
	public class BarcodeSearchCriteria
	{
		public string Barcode { get; set; }

		public BarcodeType BarcodeType { get; set; }
	}
}