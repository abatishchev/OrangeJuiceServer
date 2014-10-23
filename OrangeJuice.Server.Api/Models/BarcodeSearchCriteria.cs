using FluentValidation.Attributes;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Api.Models
{
	[Validator(typeof(Validation.BarcodeSearchCriteriaValidator))]
	public class BarcodeSearchCriteria
	{
		public string Barcode { get; set; }

		public BarcodeType BarcodeType { get; set; }
	}
}