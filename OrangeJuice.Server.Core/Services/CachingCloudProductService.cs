using System;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Services
{
	public sealed class CachingCloudProductService : IProductService
	{
		#region Fields
		private readonly IProductRepository _productRepository;
		private readonly IAzureProductProvider _azureProvider;
		private readonly IAwsProductProvider _awsProvider;
		#endregion

		#region Ctor
		public CachingCloudProductService(IAwsProductProvider awsProvider, IAzureProductProvider azureProvider, IProductRepository productRepository)
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

		public async Task<ProductDescriptor[]> Search(string barcode, BarcodeType barcodeType)
		{
			IProduct[] products = _productRepository.Search(barcode, barcodeType).ToArray();
			if (products.Any())
				return await Task.WhenAll(products.Select(p => _azureProvider.Get(p.ProductId)));

			ProductDescriptor[] descriptors = (await _awsProvider.Search(barcode, barcodeType)).ToArray();
			if (!descriptors.Any())
				return null;

			return await Task.WhenAll(descriptors.Select(d => Save(d, barcode, barcodeType)).ToArray());
		}
		#endregion

		#region Methods
		private async Task<ProductDescriptor> Save(ProductDescriptor descriptor, string barcode, BarcodeType barcodeType)
		{
			Guid productId = await _productRepository.Save(barcode, barcodeType, descriptor.SourceProductId);
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