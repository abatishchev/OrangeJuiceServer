using FluentValidation;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Validation
{
	// ReSharper disable InconsistentNaming
	public sealed class BarcodeSearchCriteriaValidator : AbstractValidator<BarcodeSearchCriteria>
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