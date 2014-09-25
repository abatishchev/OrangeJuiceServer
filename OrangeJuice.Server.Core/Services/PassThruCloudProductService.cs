using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

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
		public Task<ProductDescriptor> Get(Guid productId)
		{
			return _azureProvider.Get(productId);
		}

		public async Task<IEnumerable<Task<ProductDescriptor>>> Search(string barcode, BarcodeType barcodeType)
		{
			IEnumerable<ProductDescriptor> descriptors = await _awsProvider.Search(barcode, barcodeType);
			return descriptors.Select(Task.FromResult);
		}
		#endregion

		#region IDisposable members
		public void Dispose()
		{
		}
		#endregion
	}
}