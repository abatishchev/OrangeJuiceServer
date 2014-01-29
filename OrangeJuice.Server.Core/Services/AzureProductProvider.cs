using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using OrangeJuice.Server.Data;

namespace OrangeJuice.Server.Services
{
	public sealed class AzureProductProvider : IProductProvider
	{
		#region Fields
		private readonly IAzureClient _client;
		private readonly IProductDescriptorFactory<string> _factory;
		#endregion

		#region Ctor
		public AzureProductProvider(IAzureClient client, IProductDescriptorFactory<string> factory)
		{
			_client = client;
			_factory = factory;
		}
		#endregion

		#region IProductProvider members
		public async Task<ProductDescriptor> SearchId(Guid productId)
		{
			try
			{
				string blobContent = await _client.GetBlobFromContainer("products", productId.ToString());
				return _factory.Create(blobContent);
			}
			catch (Microsoft.WindowsAzure.Storage.StorageException)
			{
				return null;
			}
		}

		public Task<IEnumerable<ProductDescriptor>> SearchTitle(string title)
		{
			throw new NotImplementedException();
		}

		public Task<ProductDescriptor> SearchBarcode(string barcode, BarcodeType barcodeType)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}