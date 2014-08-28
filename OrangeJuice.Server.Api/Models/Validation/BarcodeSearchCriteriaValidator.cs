using FluentValidation;

namespace OrangeJuice.Server.Api.Models.Validation
{
	public sealed class BarcodeSearchCriteriaValidator : AbstractValidator<Models.BarcodeSearchCriteria>
	{
		private const int EANLength = 13;
		private const int UPCLength = 12;

		public BarcodeSearchCriteriaValidator()
		{
			RuleFor(x => x.Barcode).NotNull();
			RuleFor(x => x.Barcode).Length(UPCLength, EANLength);

			RuleFor(x => x.BarcodeType).NotEmpty();
		}
	}
}