using Ab.Amazon.Data;
using FluentValidation;

namespace OrangeJuice.Server.Data.Models.Validation
{
	public sealed class BarcodeSearchCriteriaValidator : AbstractValidator<BarcodeSearchCriteria>
	{
		public BarcodeSearchCriteriaValidator()
		{
			RuleFor(x => x.Barcode).NotNull();
			RuleFor(x => x.BarcodeType).NotEmpty();
			RuleFor(x => x).Cascade(CascadeMode.StopOnFirstFailure)
				.Must(c =>
					  {
						  if (c.Barcode != null)
						  {
							  switch (c.BarcodeType)
							  {
								  case BarcodeType.EAN:
									  return c.Barcode.Length == 13;
								  case BarcodeType.UPC:
									  return c.Barcode.Length == 12;
							  }
						  }
						  return false;
					  })
				.WithName("BarcodeLength");
		}
	}
}