using System;
using System.Collections.Generic;
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

		public async Task<IEnumerable<ProductDescriptor>> Search(string barcode, BarcodeType barcodeType)
		{
			IProduct[] products = _productRepository.Search(barcode, barcodeType).ToArray();
			if (products.Any())
				return products.Select(async p => await _azureProvider.Get(p.ProductId)).Select(t => t.Result);

			ProductDescriptor[] descriptors = (await _awsProvider.Search(barcode, barcodeType)).ToArray();
			if (!descriptors.Any())
				return null;

			return descriptors.Select(async d => await Save(d, barcode, barcodeType)).Select(t => t.Result);
		}
		#endregion

		#region Methods
		private async Task<ProductDescriptor> Save(ProductDescriptor descriptor, string barcode, BarcodeType barcodeType)
		{
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