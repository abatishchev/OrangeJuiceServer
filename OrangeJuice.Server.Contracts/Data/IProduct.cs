using System;

namespace OrangeJuice.Server.Data
{
	public interface IProduct
	{
		Guid ProductId { get; }

		string Barcode { get; set; }

		BarcodeType BarcodeType { get; set; }
	}
}