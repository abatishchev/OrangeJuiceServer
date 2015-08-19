using System;
using System.Threading.Tasks;

using Ab.Amazon;
using Ab.Amazon.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class PassThruCloudProductService : IProductService
	{
		#region Fields
		private readonly IAwsProductProvider _awsProvider;
		private readonly IAzureProductProvider _azureProvider;

		#endregion

		#region Ctor
		public PassThruCloudProductService(IAwsProductProvider awsProvider, IAzureProductProvider azureProvider)
		{
			_awsProvider = awsProvider;
			_azureProvider = azureProvider;
		}

		#endregion

		#region IProductService members
		public Task<Product> Get(Guid productId)
		{
			return _azureProvider.Get(productId);
		}

		public Task<Product[]> Search(string barcode, BarcodeType barcodeType)
		{
			return _awsProvider.Search(barcode, barcodeType);
		}
		#endregion
	}
}