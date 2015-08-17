using System;
using System.Linq;
using System.Threading.Tasks;

using Ab.Amazon.Data;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Models;

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
			Product[] products = await _productRepository.Search(barcode, barcodeType);
			if (products.Any())
				return await Task.WhenAll(products.Select(p => _azureProvider.Get(p.ProductId)));

			ProductDescriptor[] descriptors = await _awsProvider.Search(barcode, barcodeType);
			if (!descriptors.Any())
				return null;
			return await Save(descriptors, barcode, barcodeType);
		}

		#endregion

		#region Methods
		private async Task<ProductDescriptor[]> Save(ProductDescriptor[] descriptors, string barcode, BarcodeType barcodeType)
		{
			var tasks = descriptors.Select(async d => await Save(d, barcode, barcodeType));
			await Task.WhenAll(tasks);
			return descriptors;
		}

		private async Task Save(ProductDescriptor d, string barcode, BarcodeType barcodeType)
		{
			Guid productId = await _productRepository.Save(barcode, barcodeType, d.SourceProductId);
			d.ProductId = productId;
			await _azureProvider.Save(d);
		}
		#endregion
	}
}