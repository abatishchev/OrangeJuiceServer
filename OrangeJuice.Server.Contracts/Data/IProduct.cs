using System;

namespace OrangeJuice.Server.Data
{
	public interface IProduct
	{
		Guid ProductId { get; }

		string Barcode { get; }

		BarcodeType BarcodeType { get; }

		string SourceProductId { get; }
	}
}