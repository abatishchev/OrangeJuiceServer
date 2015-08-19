using System;
using System.Linq;
using System.Threading.Tasks;

using Ab.Amazon;
using Ab.Amazon.Data;
using AwsProduct = Ab.Amazon.Data.Product;

using OrangeJuice.Server.Data;
using Product = OrangeJuice.Server.Data.Models.Product;

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
		public Task<AwsProduct> Get(Guid productId)
		{
			return _azureProvider.Get(productId);
		}

		public async Task<AwsProduct[]> Search(string barcode, BarcodeType barcodeType)
		{
			Product[] products = await _productRepository.Search(barcode, barcodeType);
			if (products.Any())
				return await Task.WhenAll(products.Select(p => _azureProvider.Get(p.ProductId)));

			AwsProduct[] awsProduts = await _awsProvider.Search(barcode, barcodeType);
			if (!awsProduts.Any())
				return null;
			return await Save(awsProduts, barcode, barcodeType);
		}

		#endregion

		#region Methods
		private async Task<AwsProduct[]> Save(AwsProduct[] products, string barcode, BarcodeType barcodeType)
		{
			var tasks = products.Select(async d => await Save(d, barcode, barcodeType));
			await Task.WhenAll(tasks);
			return products;
		}

		private async Task Save(AwsProduct product, string barcode, BarcodeType barcodeType)
		{
			Guid productId = await _productRepository.Save(barcode, barcodeType, product.SourceProductId);
			product.ProductId = productId;
			await _azureProvider.Save(product);
		}
		#endregion
	}
}