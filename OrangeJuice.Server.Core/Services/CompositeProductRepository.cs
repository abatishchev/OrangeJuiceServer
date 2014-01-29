using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	// TODO: rename to avoid naming ambiguity
	public sealed class CompositeProductRepository : IProductRepository
	{
		#region Fields
		private readonly Data.Repository.IProductRepository _producDataRepository;
		private readonly IProductProvider _azureProvider;
		private readonly IProductProvider _awsProvider;
		#endregion

		#region Ctor
		public CompositeProductRepository(Data.Repository.IProductRepository producDataRepository, IProductProvider azureProvider, IProductProvider awsProvider)
		{
			_producDataRepository = producDataRepository;
			_azureProvider = azureProvider;
			_awsProvider = awsProvider;
		}
		#endregion

		#region IProductRepository members
		public async Task<IEnumerable<ProductDescriptor>> Search(string title)
		{
			return await new[] { _azureProvider, _awsProvider }.Select(p => p.SearchTitle(title))
															   .FirstOrDefaultAsync(d => d != null);
		}

		public async Task<ProductDescriptor> Lookup(string barcode, BarcodeType barcodeType)
		{
			// TODO: refactor out the flow

			IProduct product = await _producDataRepository.Search(barcode, barcodeType);
			if (product != null)
				return await _azureProvider.SearchId(product.ProductId);

			ProductDescriptor descriptor = await _awsProvider.SearchBarcode(barcode, barcodeType);

			Task.Factory.StartNew(() => SaveProduct(descriptor));

			return descriptor;
		}
		#endregion

		#region Methods
		private void SaveProduct(ProductDescriptor descriptor)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}