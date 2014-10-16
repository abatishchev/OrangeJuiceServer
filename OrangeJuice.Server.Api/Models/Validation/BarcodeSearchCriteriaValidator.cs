using FluentValidation;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Api.Models.Validation
{
	public sealed class BarcodeSearchCriteriaValidator : AbstractValidator<BarcodeSearchCriteria>
	{
		public BarcodeSearchCriteriaValidator()
		{
			CascadeMode = CascadeMode.StopOnFirstFailure;

			RuleFor(x => x.Barcode).NotNull();
			RuleFor(x => x.BarcodeType).NotEmpty();
			RuleFor(x => x).Must(c =>
				{
					switch (c.BarcodeType)
					{
						case BarcodeType.EAN:
							return c.Barcode.Length == 13;
						case BarcodeType.UPC:
							return c.Barcode.Length == 12;
						default:
							return false;
					}
				})
				.WithName("BarcodeTypeLength");
		}
	}
}