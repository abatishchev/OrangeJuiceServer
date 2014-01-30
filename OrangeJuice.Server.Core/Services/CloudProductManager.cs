﻿using System.Threading.Tasks;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Services
{
	public sealed class CloudProductManager : IProductManager
	{
		#region Fields
		private readonly IProductRepository _productRepository;
		private readonly IAzureProductProvider _azureProvider;
		private readonly IAwsProductProvider _awsProvider;
		#endregion

		#region Ctor
		public CloudProductManager(IProductRepository productRepository, IAzureProductProvider azureProvider, IAwsProductProvider awsProvider)
		{
			_productRepository = productRepository;
			_azureProvider = azureProvider;
			_awsProvider = awsProvider;
		}
		#endregion

		#region IProductManager members
		public async Task<ProductDescriptor> Search(string barcode, BarcodeType barcodeType)
		{
			IProduct product = await _productRepository.Search(barcode, barcodeType);
			if (product != null)
				return await _azureProvider.Get(product.ProductId);

			ProductDescriptor descriptor = await _awsProvider.Search(barcode, barcodeType);

			await SaveProduct(descriptor, barcode, barcodeType);

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