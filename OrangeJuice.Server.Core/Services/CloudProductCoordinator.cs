using System.Threading.Tasks;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Repository;

namespace OrangeJuice.Server.Services
{
	public sealed class CloudProductCoordinator : IProductCoordinator
	{
		#region Fields
		private readonly IProductRepository _productRepository;
		private readonly IAzureProductProvider _azureProvider;
		private readonly IAwsProductProvider _awsProvider;
		#endregion

		#region Ctor
		public CloudProductCoordinator(IProductRepository productRepository, IAzureProductProvider azureProvider, IAwsProductProvider awsProvider)
		{
			_productRepository = productRepository;
			_azureProvider = azureProvider;
			_awsProvider = awsProvider;
		}
		#endregion

		#region IProductCoordinator members
		public async Task<ProductDescriptor> Search(string barcode, BarcodeType barcodeType)
		{
			IProduct product = await _productRepository.Search(barcode, barcodeType);
			if (product != null)
				return await _azureProvider.Get(product.ProductId);

			ProductDescriptor descriptor = await _awsProvider.Search(barcode, barcodeType);

			await Task.Factory.StartNew(() => SaveProduct(descriptor, barcode, barcodeType));

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