using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Services
{
	// TODO: rename to avoid naming ambiguity
	public sealed class CloudProductCoordinator : IProductCoordinator
	{
		#region Fields
		private readonly IProductRepository _productRepository;
		private readonly IProductProvider _azureProvider;
		private readonly IProductProvider _awsProvider;
		#endregion

		#region Ctor
		public CloudProductCoordinator(IProductRepository productRepository, IProductProvider azureProvider, IProductProvider awsProvider)
		{
			_productRepository = productRepository;
			_azureProvider = azureProvider;
			_awsProvider = awsProvider;
		}
		#endregion

		#region IProductCoordinator members
		public async Task<IEnumerable<ProductDescriptor>> Search(string title)
		{
			return await new[] { _azureProvider, _awsProvider }.Select(p => p.SearchTitle(title))
															   .FirstOrDefaultAsync(d => d != null);
		}

		public async Task<ProductDescriptor> Lookup(string barcode, BarcodeType barcodeType)
		{
			IProduct product = await _productRepository.Search(barcode, barcodeType);
			if (product != null)
				return await _azureProvider.SearchId(product.ProductId);

			ProductDescriptor descriptor = await _awsProvider.SearchBarcode(barcode, barcodeType);

			Task.Factory.StartNew(() => SaveProduct(descriptor, barcode, barcodeType));

			return descriptor;
		}
		#endregion

		#region Methods
		private async Task SaveProduct(ProductDescriptor descriptor, string barcode, BarcodeType barcodeType)
		{
			IProduct product = await _productRepository.Save(barcode, barcodeType);

			descriptor.ProductId = product.ProductId;

			await _azureProvider.Save(descriptor);
		}
		#endregion
	}
}