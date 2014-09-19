using System;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Services
{
	public sealed class CloudProductService : IProductService
	{
		#region Fields
		private readonly IProductRepository _productRepository;
		private readonly IAzureProductProvider _azureProvider;
		private readonly IAwsProductProvider _awsProvider;
		#endregion

		#region Ctor
		public CloudProductService(IAwsProductProvider awsProvider, IAzureProductProvider azureProvider, IProductRepository productRepository)
		{
			_productRepository = productRepository;
			_azureProvider = azureProvider;
			_awsProvider = awsProvider;
		}
		#endregion

		#region IProductService members
		public Task<ProductDescriptor> Get(Guid productId)
		{
			return _azureProvider.Get(productId);
		}

		public async Task<ProductDescriptor> Search(string barcode, BarcodeType barcodeType)
		{
			IProduct product = await _productRepository.Search(barcode, barcodeType);
			if (product != null)
				return await _azureProvider.Get(product.ProductId);

			ProductDescriptor descriptor = await _awsProvider.Search(barcode, barcodeType);
			if (descriptor == null)
				return null;

			Guid productId = await _productRepository.Save(barcode, barcodeType);
			descriptor.ProductId = productId;
			await _azureProvider.Save(descriptor);

			return descriptor;
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
			_productRepository.Dispose();
		}
		#endregion
	}
}