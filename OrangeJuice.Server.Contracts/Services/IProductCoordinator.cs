﻿using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public interface IProductCoordinator
	{
		Task<ProductDescriptor> Lookup(string barcode, BarcodeType barcodeType);
	}
}